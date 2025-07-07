using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Operations.User.Dtos;
using CafeManagementSystem.Web.Helpers;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.Web.Controllers
{
    public class OrderController : BaseController
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
            // Ürün ve kullanıcı listelerini yükle
            var products = await _api.GetAsync<List<ProductDto>>("api/Products");
            var users = await _api.GetAsync<List<UserInfoDto>>("api/Users");

            ViewBag.Products = products;
            ViewBag.Users = users;

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
                    ModelState.AddModelError("", "Sipariş oluşturulamadı: " + ex.Message);
                }
            }

            // Hata durumunda listeleri tekrar yükle
            ViewBag.Products = await _api.GetAsync<List<ProductDto>>("api/Products");
            ViewBag.Users = await _api.GetAsync<List<UserInfoDto>>("api/Users");

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _api.GetAsync<OrderDto>($"api/Orders/{id}");
            var products = await _api.GetAsync<List<ProductDto>>("api/Products");
            
            var updateDto = new UpdateOrderDto
            {
                Id = order.Id,
                ProductId = order.ProductId,
                GuestCount = order.GuestCount,
                IsConfirmed = order.IsConfirmed,
                SpecialRequest = order.SpecialRequest
            };
            
            ViewBag.Products = products;
            return View(updateDto);
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
        [HttpPatch]
        public async Task<IActionResult> Confirm(int id)
        {
            await _api.PatchAsync($"api/Orders/{id}/IsConfirmed", true);
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _api.GetAsync<OrderDto>($"api/Orders/{id}");
            return View(order);
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

