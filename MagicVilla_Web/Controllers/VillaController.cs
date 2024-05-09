using AutoMapper;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDto> list = new();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result));
            }

            return View(list);
        }
        public async Task<IActionResult> CreateVilla()
        {


            return View();
        }
        [HttpPost, ActionName("CreateVilla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaPost(VillaDtoCreate villaDtoCreate)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(villaDtoCreate);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVilla");
                }

            }



            return View(villaDtoCreate);
        }
        public async Task<IActionResult> UpdateVilla(int id)
        {
            var response = await _villaService.GetAsync<APIResponse>(id);


            if (response != null && response.IsSuccess)
            {
                VillaDto villa = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaDtoUpdate>(villa));
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaDtoUpdate villaDtoUpdate)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(villaDtoUpdate);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVilla");
                }

            }



            return View(villaDtoUpdate);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            var response = await _villaService.GetAsync<APIResponse>(id);


            if (response != null && response.IsSuccess)
            {
                VillaDto villa = JsonConvert.DeserializeObject<VillaDto>(Convert.ToString(response.Result));

                return View(villa);
            }
            return NotFound();
        }
        [HttpPost, ActionName("DeleteVilla")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDto villaDtoDelete)
        {

            var response = await _villaService.DeleteAsync<APIResponse>(villaDtoDelete.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("IndexVilla");
            }
            return View(villaDtoDelete);
        }
    }
}
