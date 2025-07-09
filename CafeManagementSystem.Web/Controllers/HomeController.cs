using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CafeManagementSystem.Web.Models;
using CafeManagementSystem.Web.Controllers;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Web.Services;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using static CafeManagementSystem.Web.Services.ApiService;

namespace CafeManagementSystem.Web.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApiService _api;

    public HomeController(ILogger<HomeController>
        logger, ApiService api)
    {
        _logger = logger;
        _api = api;
    }

    public async Task<IActionResult> Index()
    {    
      
            return View();

    }


    public async Task<IActionResult> CafeProducts(int id)
    {
        var products = await _api.GetAsync<List<ProductDto>>($"api/Orders/Index");
        ViewBag.CafeId = id;
        return View("CafeProducts", products);
    }

    [HttpGet]
    public IActionResult Maintenance()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

