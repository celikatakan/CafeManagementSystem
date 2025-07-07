using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CafeManagementSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using CafeManagementSystem.Business.Operations.Order.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;

namespace CafeManagementSystem.Web.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly ApiService _api;

        public DashboardController(ApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                // Dashboard verilerini yükle
                var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
                var products = await _api.GetAsync<List<ProductDto>>("api/Products");
                var cafes = await _api.GetAsync<List<CafeDto>>("api/Cafes");

                // İstatistikleri hesapla
                var stats = CalculateStatistics(orders, products, cafes);

                return View(stats);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş istatistikler göster
                return View(new DashboardStats());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderStats()
        {
            try
            {
                var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
                var stats = CalculateOrderStats(orders);
                return Json(stats);
            }
            catch
            {
                return Json(new { error = "Veri yüklenemedi" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenueStats()
        {
            try
            {
                var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
                var revenue = CalculateRevenueStats(orders);
                return Json(revenue);
            }
            catch
            {
                return Json(new { error = "Veri yüklenemedi" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPopularProducts()
        {
            try
            {
                var orders = await _api.GetAsync<List<OrderDto>>("api/Orders");
                var products = await _api.GetAsync<List<ProductDto>>("api/Products");
                var popular = CalculatePopularProducts(orders, products);
                return Json(popular);
            }
            catch
            {
                return Json(new { error = "Veri yüklenemedi" });
            }
        }

        private DashboardStats CalculateStatistics(List<OrderDto> orders, List<ProductDto> products, List<CafeDto> cafes)
        {
            var stats = new DashboardStats();

            if (orders != null)
            {
                stats.TotalOrders = orders.Count;
                stats.PendingOrders = orders.FindAll(o => !o.IsConfirmed).Count;
                stats.CompletedOrders = orders.FindAll(o => o.IsConfirmed).Count;
                stats.TotalRevenue = orders.FindAll(o => o.IsConfirmed).Sum(o => o.GuestCount * 50); // Ortalama 50 TL
            }

            if (products != null)
            {
                stats.TotalProducts = products.Count;
            }

            if (cafes != null)
            {
                stats.TotalCafes = cafes.Count;
            }

            return stats;
        }

        private object CalculateOrderStats(List<OrderDto> orders)
        {
            if (orders == null) return new { labels = new string[0], data = new int[0] };

            var today = DateTime.Today;
            var last7Days = new List<string>();
            var orderCounts = new List<int>();

            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                last7Days.Add(date.ToString("dd/MM"));

                var dayOrders = orders.FindAll(o => o.CreatedDate.Date == date);
                orderCounts.Add(dayOrders.Count);
            }

            return new
            {
                labels = last7Days.ToArray(),
                data = orderCounts.ToArray()
            };
        }

        private object CalculateRevenueStats(List<OrderDto> orders)
        {
            if (orders == null) return new { labels = new string[0], data = new decimal[0] };

            var today = DateTime.Today;
            var last7Days = new List<string>();
            var revenues = new List<decimal>();

            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                last7Days.Add(date.ToString("dd/MM"));

                var dayOrders = orders.FindAll(o => o.CreatedDate.Date == date && o.IsConfirmed);
                var dayRevenue = dayOrders.Sum(o => o.GuestCount * 50); // Ortalama 50 TL
                revenues.Add(dayRevenue);
            }

            return new
            {
                labels = last7Days.ToArray(),
                data = revenues.ToArray()
            };
        }

        private object CalculatePopularProducts(List<OrderDto> orders, List<ProductDto> products)
        {
            if (orders == null || products == null) 
                return new { labels = new string[0], data = new int[0] };

            var productCounts = new Dictionary<int, int>();

            foreach (var order in orders)
            {
                var productId = order.ProductId;
                if (productCounts.ContainsKey(productId))
                    productCounts[productId]++;
                else
                    productCounts[productId] = 1;
            }

            var topProducts = productCounts
                .OrderByDescending(x => x.Value)
                .Take(5)
                .ToList();

            var labels = new List<string>();
            var data = new List<int>();

            foreach (var product in topProducts)
            {
                var productInfo = products.Find(p => p.Id == product.Key);
                if (productInfo != null)
                {
                    labels.Add(productInfo.ProductName);
                    data.Add(product.Value);
                }
            }

            return new
            {
                labels = labels.ToArray(),
                data = data.ToArray()
            };
        }
    }

    public class DashboardStats
    {
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCafes { get; set; }
    }
} 