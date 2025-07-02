using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApiService _api;

        public ProductController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _api.GetAsync<List<ProductDto>>("api/Products");
            return View(products);
        }
        [HttpGet]   
        public async Task<IActionResult> Create()
        {
            return View(new AddProductDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddProductDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PostAsync("api/Products", model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // API'den dönen hatayı kullanıcıya göster
                    ModelState.AddModelError("Ürün", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cafe = await _api.GetAsync<UpdateProductDto>($"api/Products/{id}");
            return View(cafe);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PutAsync($"api/Products/{model.Id}", model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Güncelleme sırasında hata oluştu: " + ex.Message);
                }
            }
          
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _api.DeleteAsync($"api/Products/{id}");
            // Redirect to another page after deletion
            return RedirectToAction("Index");
        }
    }
}

