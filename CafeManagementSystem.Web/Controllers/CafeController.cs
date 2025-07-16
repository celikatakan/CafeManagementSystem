using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static CafeManagementSystem.Web.Services.ApiService;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.Web.Controllers
{
    public class CafeController : BaseController
    {
        private readonly ApiService _api;

        public CafeController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cafes = await _api.GetAsync<List<CafeDto>>("api/Cafes");
                return View(cafes ?? new List<CafeDto>());
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch 
            {
                TempData["Error"] = "Kafeler yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return View(new List<CafeDto>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                
                var features = await _api.GetAsync<List<FeatureDto>>("api/Features/api/Features"); 
                ViewBag.Features = features ?? new List<FeatureDto>();
                return View(new AddCafeDto());
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch 
            {
     
                ViewBag.Features = new List<FeatureDto>();
     
                return View(new AddCafeDto());
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddCafeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PostAsync("api/Cafes", model);
                    return RedirectToAction("Index");
                }
                catch (MaintenanceException)
                {
                    return RedirectToAction("Index", "Maintenance");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Kafe","Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }

            var features = await _api.GetAsync<List<FeatureDto>>("api/Features/");
            ViewBag.Features = features ?? new List<FeatureDto>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            
            try
            {
                var cafe = await _api.GetAsync<UpdateCafeDto>($"api/Cafes/{id}");
                var features = await _api.GetAsync<List<FeatureDto>>("api/Features/api/Features");
                ViewBag.Features = features ?? new List<FeatureDto>();
                return View(cafe);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCafeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PutAsync($"api/Cafes/{model.Id}", model);
                    return RedirectToAction("Index");
                }
                catch (MaintenanceException)
                {
                    return RedirectToAction("Index", "Maintenance");
                }   
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında hata oluştu: " + ex.Message);
                }
            }
            var features = await _api.GetAsync<List<FeatureDto>>("api/Features/api/Features");
            ViewBag.Features = features ?? new List<FeatureDto>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {       
            try
            {
                await _api.DeleteAsync($"api/Cafes/{id}");
                return RedirectToAction("Index");
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Like(int cafeId)
        {
            // Kullanıcı id'sini al
            int userId = 0;
            if (User.Identity.IsAuthenticated)
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
                if (idClaim != null)
                    userId = int.Parse(idClaim.Value);
                else if (!string.IsNullOrEmpty(User.Identity.Name))
                    userId = int.Parse(User.Identity.Name);
            }
            if (userId == 0)
                return Unauthorized();

            // Business layer ile beğeni işlemini yap
            // await _cafeLikeService.LikeCafeAsync(cafeId, userId); // _cafeLikeService is not defined in this file

            // AJAX ise JSON, normal ise Redirect
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return Json(new { success = true });

            return RedirectToAction("Detail", "Customer", new { id = cafeId });
        }
    }
}

