using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApiService _api;

        public OrderController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
            return View(orders);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View(new CreateOrderDto());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PostAsync("api/Orders", model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // API'den dönen hatayı kullanıcıya göster
                    ModelState.AddModelError("Sipariş", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _api.GetAsync<UpdateOrderDto>($"api/Orders/{id}");
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateOrderDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _api.PutAsync($"api/Orders/{model.Id}", model);
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
            await _api.DeleteAsync($"api/Orders/{id}");
            // Redirect to another page after deletion
            return RedirectToAction("Index");
        }
    }
}

