using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebApi_test.Data;
using WebApi_test.Model;
using WebApi_test.Model.Dto;
using WebApi_test.Repository.IRepository;

namespace WebApi_test.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository villaRepository, IMapper mapper)
        {
            this._response = new();
            _villaRepository = villaRepository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {

                var villasList = await _villaRepository.AllAsync();
                _response.Result = _mapper.Map<List<VillaDto>>(villasList);
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

        [HttpGet("{id:int}", Name = "Getvilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> GetVillaAsync(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                var villas = await _villaRepository.GetAsync(x => x.Id == id);
                if (villas == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return _response;

                }

                _response.Result = _mapper.Map<VillaDto>(villas);
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaAsync([FromBody] VillaDtoCreate createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;

                }
                //if (villaDto == null)
                //{
                //    return BadRequest(villaDto);
                //    ModelState.AddModelError()
                //}
                if (await _villaRepository.GetAsync(u => u.Name.ToLower() == createDto.Name.ToLower()) != null)
                {
                    ModelState.AddModelError("name", "the value already exist");

                    return BadRequest(ModelState);
                }


                var villa = _mapper.Map<Villa>(createDto);
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
                await _villaRepository.CreateAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                _response.IsSuccess = true;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ErrorMessages = new List<string>() { ex.Message };
                _response.IsSuccess = false;
            }
            return _response;


        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaRepository.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    NotFound();
                }
                await _villaRepository.RemoveAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
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

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateVillaAsync(int id, [FromBody] VillaDtoUpdate UpdateDto)
        {
            try
            {
                if (id == 0 || id != UpdateDto.Id)
                {
                    return BadRequest();
                }

                var villa = _mapper.Map<Villa>(UpdateDto);
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
                await _villaRepository.UpdateAsync(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
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

        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdatePartialVillaAsync(int id, JsonPatchDocument<VillaDtoUpdate> patch)
        {
            if (id == 0 || patch == null)
            {
                return BadRequest();
            }

            var villa = await _villaRepository.GetAsync(u => u.Id == id, tracked: false);

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
            villa = _mapper.Map<Villa>(villaDtoUpdate);
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
            await _villaRepository.UpdateAsync(villa);

            return NoContent();
        }

    }
}
