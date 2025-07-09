using Microsoft.AspNetCore.Mvc;
using CafeManagementSystem.Web.Services;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using static CafeManagementSystem.Web.Services.ApiService;

namespace CafeManagementSystem.Web.Controllers
{
  
    public class ProductController : BaseController
    {
        private readonly ApiService _api;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ApiService api, ILogger<ProductController> logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _api.GetAsync<List<ProductDto>>("api/Products");
                return View(products ?? new List<ProductDto>());
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler listelenirken hata oluştu");
                TempData["Error"] = "Ürünler yüklenirken bir hata oluştu. Lütfen daha sonra tekrar deneyin.";
                return View(new List<ProductDto>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new AddProductDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PostAsync("api/Products", model);
                TempData["Success"] = "Ürün başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün eklenirken hata oluştu");
                ModelState.AddModelError("", "Ürün eklenirken bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var product = await _api.GetAsync<UpdateProductDto>($"api/Products/{id}");
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün düzenleme sayfası açılırken hata oluştu");
                TempData["Error"] = "Ürün bilgileri yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _api.PutAsync($"api/Products/{model.Id}", model);
                TempData["Success"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncellenirken hata oluştu");
                ModelState.AddModelError("", "Güncelleme sırasında hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _api.DeleteAsync($"api/Products/{id}");
                TempData["Success"] = "Ürün başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (MaintenanceException)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata oluştu");
                TempData["Error"] = "Ürün silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}