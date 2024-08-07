﻿using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Model.Dto;
using WebApi_test.Repository.IRepository;

namespace WebApi_test.Controllers.v2
{

    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]

    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _villaNumberRepository;
        private readonly IMapper _mapper;
        private readonly IVillaRepository _villaRepository;
        public VillaNumberAPIController(IVillaNumberRepository villaNumberRepository, IMapper mapper, IVillaRepository villaRepository)
        {
            _response = new();
            _villaNumberRepository = villaNumberRepository;
            _mapper = mapper;
            _villaRepository = villaRepository;

        }
        [HttpGet("GetString")]
        public IEnumerable<string> Get()
        {

            return new string[] { "keketso", "Moloi" };
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ResponseCache(CacheProfileName = "CachProfile")]

        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {

                var villaNumberList = await _villaNumberRepository.AllAsync(includeProperties: "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetvillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> GetVillaNumberAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                var villaNumber = await _villaNumberRepository.GetAsync(x => x.VillaNo == id);
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;

                }

                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumberAsync([FromBody] VillaNumberDtoCreate createDto)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    _response.StatusCode = HttpStatusCode.BadRequest;
                //    return _response;

                //}
                //if (villaDto == null)
                //{
                //    return BadRequest(villaDto);
                //    ModelState.AddModelError()
                //}
                if (await _villaNumberRepository.GetAsync(u => u.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("ErrorMessages", "the value already exist");

                    return BadRequest(ModelState);
                }
                var villa = await _villaRepository.GetAsync(u => u.Id == createDto.VillaId);
                if (villa == null)
                {
                    ModelState.AddModelError("ErrorMessages", "the id those not exist");

                    return BadRequest(ModelState);
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDto);
                //var villa = new Villa
                //{

                //    Amenity = villaDto.Amenity,
                //    Details = villaDto.Details,

                //    ImageUrl = villaDto.ImageUrl,
                //    Name = villaDto.Name,
                //    Occupancy = villaDto.Occupancy,
                //    Rate = villaDto.Rate,
                //    Sqft = villaDto.Sqft,
                //};
                await _villaNumberRepository.CreateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;


        }

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villaNumber = await _villaNumberRepository.GetAsync(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    NotFound();
                }
                await _villaNumberRepository.RemoveAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;


        }

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody] VillaNumberDtoUpdate UpdateDto)
        {
            try
            {
                if (id == 0 || id != UpdateDto.VillaNo)
                {
                    return BadRequest();
                }

                if (await _villaRepository.GetAsync(u => u.Id == UpdateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "the id those not exist");

                    return BadRequest(ModelState);
                }

                var villaNumber = _mapper.Map<VillaNumber>(UpdateDto);
                //var villa = new Villa
                //{

                //    Amenity = UpdateDto.Amenity,
                //    Details = UpdateDto.Details,
                //    Id = UpdateDto.Id,
                //    ImageUrl = UpdateDto.ImageUrl,
                //    Name = UpdateDto.Name,
                //    Occupancy = UpdateDto.Occupancy,
                //    Rate = UpdateDto.Rate,
                //    Sqft = UpdateDto.Sqft,
                //};
                await _villaNumberRepository.UpdateAsync(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;


        }

        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaDtoUpdate> patch)
        {
            if (id == 0 || patch == null)
            {
                return BadRequest();
            }

            var villa = await _villaNumberRepository.GetAsync(u => u.VillaNo == id, tracked: false);

            if (villa == null)
            {
                return BadRequest();
            }
            var villaDtoUpdate = _mapper.Map<VillaDtoUpdate>(villa);
            //var villaDtoUpdate = new VillaDtoUpdate
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    Id = villa.Id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft,
            //};

            patch.ApplyTo(villaDtoUpdate, ModelState);
            villa = _mapper.Map<VillaNumber>(villaDtoUpdate);
            //villa = new Villa
            //{

            //    Amenity = villaDtoUpdate.Amenity,
            //    Details = villaDtoUpdate.Details,
            //    Id = villaDtoUpdate.Id,
            //    ImageUrl = villaDtoUpdate.ImageUrl,
            //    Name = villaDtoUpdate.Name,
            //    Occupancy = villaDtoUpdate.Occupancy,
            //    Rate = villaDtoUpdate.Rate,
            //    Sqft = villaDtoUpdate.Sqft,
            //};

            if (ModelState.IsValid)
            {
                return BadRequest();
            }
            await _villaNumberRepository.UpdateAsync(villa);

            return NoContent();
        }

    }
}
