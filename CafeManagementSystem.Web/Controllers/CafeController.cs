using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

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
            var cafes = await _api.GetAsync<List<CafeDto>>("api/Cafes");
            return View(cafes);
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
            catch (HttpRequestException ex)
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
                catch (Exception ex)
                {
                    // API'den dönen hatayı kullanıcıya göster
                    ModelState.AddModelError("Kafe","Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }

            // Hata durumunda özellikleri tekrar yükle
            var features = await _api.GetAsync<List<FeatureDto>>("api/Features/");
            ViewBag.Features = features ?? new List<FeatureDto>();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cafe = await _api.GetAsync<UpdateCafeDto>($"api/Cafes/{id}");
            var features = await _api.GetAsync<List<FeatureDto>>("api/Features/api/Features");
            ViewBag.Features = features ?? new List<FeatureDto>();
            return View(cafe);
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
            await _api.DeleteAsync($"api/Cafes/{id}");
            // Redirect to another page after deletion
            return RedirectToAction("Index");
        }
    }
}

