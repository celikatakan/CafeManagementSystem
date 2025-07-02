using System;
namespace CafeManagementSystem.Business.Operations.Product.Dtos
{
	public class AddProductDto
	{
        public int CafeId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}

