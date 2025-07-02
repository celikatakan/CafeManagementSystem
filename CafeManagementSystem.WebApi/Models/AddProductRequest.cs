using System;
namespace CafeManagementSystem.WebApi.Models
{
	public class AddProductRequest
	{
        public int CafeId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int? StockQuantity { get; set; }
        public string ImageUrl { get; set; }
    }
}

