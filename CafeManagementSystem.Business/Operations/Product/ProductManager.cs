using System;
using Azure.Core;
using System.Diagnostics;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Business.Operations.Product
{
    public class ProductManager : IProductService
	{
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductManager(IRepository<ProductEntity> productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceMessage> AddjustProductStock(int id, int changeTo)
        {
            var product = _productRepository.GetById(id);

            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kafe bulunamadı."
                };
            }
            product.StockQuantity = changeTo;

            _productRepository.Update(product);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Stok miktarı değiştirirken bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Stok miktarı başarıyla değiştirildi."
            };
        }

        public async Task<ServiceMessage> AddProduct(AddProductDto product)
        {
            var hasProduct = _productRepository.GetAll(x => x.ProductName.ToLower() == product.ProductName.ToLower()).Any();

            if (hasProduct)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün zaten bulunuyor."
                };
            }

            var productEntity = new ProductEntity
            {
                ProductName = product.ProductName,
                Price = product.Price,
                Category = product.Category,
                StockQuantity = product.StockQuantity,
                ImageUrl = product.ImageUrl,
                CafeId = product.CafeId
            };
            _productRepository.Add(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Ürün ekleme sırasında bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<ServiceMessage> DeleteProduct(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }
            _productRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Ürün silinirken bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Ürün başarıyla silindi."
            };
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetAll(x => x.Id == id)
                  .Select(x => new ProductDto
                  {
                           Id= x.Id,
                           ProductName= x.ProductName,
                           Price = x.Price,
                           Category = x.Category,
                           StockQuantity = x.StockQuantity,
                           ImageUrl = x.ImageUrl,
                           CafeId= x.CafeId 

                  }).FirstOrDefaultAsync();

            return product;
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var products = await _productRepository.GetAll()
                 .Select(x => new ProductDto
                 {
                     Id = x.Id,
                     ProductName = x.ProductName,
                     Price = x.Price,
                     Category = x.Category,
                     StockQuantity = x.StockQuantity,
                     ImageUrl = x.ImageUrl,
                     CafeId = x.CafeId

                 }).ToListAsync();

            return products;
        }

        public async Task<ServiceMessage> UpdateProduct(UpdateProductDto product)
        {
            var productEntity = _productRepository.GetById(product.Id);

            if (productEntity == null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Ürün bulunamadı."
                };
            }

            productEntity.CafeId = product.CafeId;
            productEntity.ProductName = product.ProductName;
            productEntity.Price = product.Price;
            productEntity.Category = product.Category;
            productEntity.StockQuantity = product.StockQuantity;
            productEntity.ImageUrl = product.ImageUrl;

            _productRepository.Update(productEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw new Exception("Ürün bilgileri güncellenirken bir hata ile karşılaşıldı.");
            }

            
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kafe başarıyla güncellendi."
            };
        }
    }
}

