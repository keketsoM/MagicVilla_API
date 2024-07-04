using AutoMapper;
using MagicVilla_Utility;
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
                    TempData["success"] = "VillaNumber created successfully";
                    return RedirectToAction("IndexVillaNumber");
                }
                else if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessages.FirstOrDefault().ToString());
                    TempData["error"] = "Error encounted";
                }
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                villaNumberCreateVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return View(villaNumberCreateVM);
        }
        public async Task<IActionResult> UpdateVillaNumber(int id)
        {
            VillaNumberUpdateVM villaNumberUpdateVM = new VillaNumberUpdateVM();
            var response = await _villaNumberService.GetAsync<APIResponse>(id);

            if (response != null && response.IsSuccess)
            {
                VillaNumberDto villaNumber = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));
                villaNumberUpdateVM.VillaNumber = _mapper.Map<VillaNumberDtoUpdate>(villaNumber);
            }
            var respo = await _villaService.GetAllAsync<APIResponse>();
            if (respo != null && respo.IsSuccess)
            {
                villaNumberUpdateVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(respo.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(villaNumberUpdateVM);
            }
            TempData["error"] = "Error encounted";
            return NotFound();
        }
        [HttpPost, ActionName("UpdateVillaNumber")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM villaNumberUpdateVM)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(villaNumberUpdateVM.VillaNumber);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "VillaNumber Updated successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else if (response.ErrorMessages.Count > 0)
                {
                    ModelState.AddModelError("ErrorMessage", response.ErrorMessages.FirstOrDefault().ToString());
                    TempData["error"] = "Error encounted";
                }
            }

            var resp = await _villaService.GetAllAsync<APIResponse>();
            if (resp != null && resp.IsSuccess)
            {
                villaNumberUpdateVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(resp.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
            }
            return View(villaNumberUpdateVM);
        }

        public async Task<IActionResult> DeleteVillaNumber(int id)
        {
            VillaNumberDeleteVM villaNumberDeleteVM = new VillaNumberDeleteVM();
            var response = await _villaNumberService.GetAsync<APIResponse>(id);

            if (response != null && response.IsSuccess)
            {
                villaNumberDeleteVM.VillaNumber = JsonConvert.DeserializeObject<VillaNumberDto>(Convert.ToString(response.Result));

            }
            var respo = await _villaService.GetAllAsync<APIResponse>();
            if (respo != null && respo.IsSuccess)
            {
                villaNumberDeleteVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(respo.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(villaNumberDeleteVM);
            }
            TempData["error"] = "Error encounted";
            return NotFound();
        }
        [HttpPost, ActionName(nameof(DeleteVillaNumber))]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaPost(VillaNumberDeleteVM villaNumberDeleteVM)
        {

            var response = await _villaNumberService.DeleteAsync<APIResponse>(villaNumberDeleteVM.VillaNumber.VillaNo);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "VillaNumber deleted successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            var respo = await _villaService.GetAllAsync<APIResponse>();
            if (respo != null && respo.IsSuccess)
            {
                villaNumberDeleteVM.villaList = JsonConvert.DeserializeObject<List<VillaDto>>(Convert.ToString(respo.Result)).Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(villaNumberDeleteVM);
            }
            TempData["error"] = "Error encounted";
            return NotFound();
        }
    }
}
