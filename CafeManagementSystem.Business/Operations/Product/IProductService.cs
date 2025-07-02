using System;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Operations.Product.Dtos;
using CafeManagementSystem.Business.Types;

namespace CafeManagementSystem.Business.Operations.Product
{
	public interface IProductService
	{
		Task<ProductDto> GetProductByIdAsync(int id);
        Task<List<ProductDto>> GetProducts();
        Task<ServiceMessage> AddProduct(AddProductDto product);
        Task<ServiceMessage> AddjustProductStock(int id, int changeTo);
        Task<ServiceMessage> UpdateProduct(UpdateProductDto cafe);
        Task<ServiceMessage> DeleteProduct(int id);

    }
}

