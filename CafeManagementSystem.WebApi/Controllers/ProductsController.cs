using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Feature;
using CafeManagementSystem.Business.Operations.Feature.Dtos;
using CafeManagementSystem.Business.Operations.Product;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var product = await _productService.GetProducts();

            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AddProductRequest request)
        {
            var addProductDto = new AddProductDto
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Category = request.Category,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
                CafeId = request.CafeId
            };

            var result = await _productService.AddProduct(addProductDto);

            if (result.IsSucceed)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
        [HttpPatch("{id}/stock")]
        public async Task<IActionResult> AddjustProductStock(int id, int changeTo)
        {
            var result = await _productService.AddjustProductStock(id, changeTo);

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductRequest request)
        {
            var updateProductDto = new UpdateProductDto
            {
                Id = id,
                CafeId = request.CafeId,
                ProductName = request.ProductName,
                Price = request.Price,
                Category = request.Category,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl
            };

            var result = await _productService.UpdateProduct(updateProductDto);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return await GetById(id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProduct(id);
            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return Ok(result);
        }
    }
}

