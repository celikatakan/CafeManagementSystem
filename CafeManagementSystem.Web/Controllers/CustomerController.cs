using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Operations.Review.Dtos;
using CafeManagementSystem.Business.Operations.User.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using static CafeManagementSystem.Web.Services.ApiService;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.Web.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly ApiService _api;

        public CustomerController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cafes = await _api.GetAsync<List<CafeDto>>("api/Cafes");
                return View(cafes);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                var cafe = await _api.GetAsync<CafeDto>($"api/Cafes/{id}");
                if (cafe == null)
                    return NotFound();

                var allProducts = await _api.GetAsync<List<ProductDto>>("api/Products");
                var products = allProducts?.Where(p => p.CafeId == id).ToList() ?? new List<ProductDto>();
                ViewBag.Products = products;

                var reviews = await _api.GetAsync<List<ReviewDto>>($"api/Reviews/cafe/{id}");
                ViewBag.Reviews = reviews ?? new List<ReviewDto>();

                return View(cafe);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }
        public async Task<IActionResult> Info()
        {
            try
            {
                var userInfo = new UserInfoDto
                {
                    Id = int.Parse(User.FindFirst("id")?.Value ?? "0"),
                    Email = User.FindFirst("email")?.Value ?? "",
                    FirstName = User.FindFirst("firstName")?.Value ?? "",
                    LastName = User.FindFirst("lastName")?.Value ?? "",
                    UserType = (CafeManagementSystem.Data.Enums.UserType)Enum.Parse(typeof(CafeManagementSystem.Data.Enums.UserType), User.FindFirst("userType")?.Value ?? "Customer"),
                    CreatedAt = DateTime.Now // TODO: Get actual creation date from API
                };

                // Get user's reviews
                var reviews = await _api.GetAsync<List<ReviewDto>>($"api/Reviews/user/{userInfo.Id}");
                if (reviews != null)
                {
                    userInfo.Reviews = reviews;
                }

                // Get user's orders
                var orders = await _api.GetAsync<List<OrderDto>>($"api/Orders/user/{userInfo.Id}");
                if (orders != null)
                {
                    userInfo.Orders = orders;
                }

                // Get user's liked cafes
                var likedCafes = await _api.GetAsync<List<CafeDto>>($"api/Users/{userInfo.Id}/likedcafes");
                ViewBag.LikedCafes = likedCafes ?? new List<CafeDto>();

                return View(userInfo);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
        }


    }
}

