using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

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
            var cafes = await _api.GetAsync<List<CafeDto>>("api/Cafes");
            return View(cafes);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var cafe = await _api.GetAsync<CafeDto>($"api/Cafes/{id}");
            if (cafe == null)
                return NotFound();

            var allProducts = await _api.GetAsync<List<ProductDto>>("api/Products");
            var products = allProducts?.Where(p => p.CafeId == id).ToList() ?? new List<ProductDto>();
            ViewBag.Products = products;

            // Yorumları çek
            var reviews = await _api.GetAsync<List<CafeManagementSystem.Business.Operations.Review.Dtos.ReviewDto>>($"api/Reviews/cafe/{id}");
            ViewBag.Reviews = reviews ?? new List<CafeManagementSystem.Business.Operations.Review.Dtos.ReviewDto>();

            return View(cafe); // CafeDto model ile
        }
    }
}

