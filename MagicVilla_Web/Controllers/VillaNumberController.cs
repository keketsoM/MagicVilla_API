using AutoMapper;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;


        }
        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDto> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
        public async Task<IActionResult> CreateVillaNumber()
        {


            return View();
        }
        [HttpPost, ActionName("CreateVillaNumber")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaPost(VillaNumberDtoCreate villaNumberDtoCreate)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumberDtoCreate);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(villaNumberDtoCreate);
        }
        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(id);

            VillaDto villa = new VillaDto();
            if (response != null && response.IsSuccess)
            {
                villa = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaDtoUpdate>(villa));
            }
            return NotFound();
        }
        [HttpPost, ActionName("UpdateVilla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaPost(VillaNumberDtoUpdate villaNumberDtoUpdate)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberDtoUpdate);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("Index");
                }

            }



            return View(villaNumberDtoUpdate);
        }

        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(id);

            VillaDto villa = new VillaDto();
            if (response != null && response.IsSuccess)
            {
                villa = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));

                return View(villa);
            }
            return NotFound();
        }
        [HttpPost, ActionName("UpdateVilla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaPost(VillaDto villaDtoDelete)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(villaDtoDelete.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(villaDtoDelete);
        }
    }
}
