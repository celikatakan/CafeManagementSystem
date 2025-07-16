using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Operations.User.Dtos;
using CafeManagementSystem.Web.Helpers;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static CafeManagementSystem.Web.Services.ApiService;

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
           
            try
            {
                var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
                return View(orders);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
  
        [HttpGet]
        public async Task<IActionResult> Create()
        {
          
            try
            {
                // Ürün ve kullanıcı listelerini yükle
                var products = await _api.GetAsync<List<ProductDto>>("api/Products");
                var users = await _api.GetAsync<List<UserInfoDto>>("api/Users");

                ViewBag.Products = products;
                ViewBag.Users = users;

                return View(new CreateOrderDto());
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto model)
        {
            
            try
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

         
                ViewBag.Products = await _api.GetAsync<List<ProductDto>>("api/Products");
                ViewBag.Users = await _api.GetAsync<List<UserInfoDto>>("api/Users");

                return View(model);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Repeat([FromBody] CreateOrderDto model)
        {
            // Kullanıcı kimliği cookie/claim'den alınacak
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            // Sadece kendi adına sipariş oluşturulabilir
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized();

            var repeatOrder = new CreateOrderDto
            {
                ProductId = model.ProductId,
                UserId = userId,
                GuestCount = model.GuestCount,
                SpecialRequest = model.SpecialRequest
            };
            try
            {
                await _api.PostAsync("api/Orders", repeatOrder);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
           
            try
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
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateOrderDto model)
        {
            
            try
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
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpPatch]
        public async Task<IActionResult> Confirm(int id)
        {
            
            try
            {
                await _api.PatchAsync($"api/Orders/{id}/IsConfirmed", true);
                return NoContent();
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
           
            try
            {
                var order = await _api.GetAsync<OrderDto>($"api/Orders/{id}");
                return View(order);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
           
            try
            {
                await _api.DeleteAsync($"api/Orders/{id}");
                return RedirectToAction("Index");
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
    }
}

