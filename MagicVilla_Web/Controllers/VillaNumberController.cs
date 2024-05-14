using AutoMapper;
using MagicVilla_Web.Model;
using MagicVilla_Web.Model.Dto;
using MagicVilla_Web.Model.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaNumberController(IVillaNumberService villaNumberService, IMapper mapper, IVillaService villaService)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;


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
            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberCreateVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(response.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }

            return View(villaNumberCreateVM);
        }
        [HttpPost, ActionName("CreateVillaNumber")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVillaPost(VillaNumberCreateVM villaNumberCreateVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(villaNumberCreateVM.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }
            }
            return View(villaNumberCreateVM);
        }
        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            var response = await _villaNumberService.GetAsync<APIResponse>(id);

            VillaNumberDto villa = new VillaNumberDto();
            if (response != null && response.IsSuccess)
            {
                villa = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));

                return View(_mapper.Map<VillaNumberDtoUpdate>(villa));
            }
            return NotFound();
        }
        [HttpPost, ActionName("UpdateVillaNumber")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberDtoUpdate villaNumberDtoUpdate)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberDtoUpdate);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction("IndexVillaNumber");
                }

            }



            return View(villaNumberDtoUpdate);
        }

        public async Task<IActionResult> DeleteVilla
            (int id)
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
        [HttpPost, ActionName(nameof(DeleteVilla))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaPost(VillaDto villaDtoDelete)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(villaDtoDelete.Id);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(DeleteVilla));
            }
            return View(villaDtoDelete);
        }
    }
}
