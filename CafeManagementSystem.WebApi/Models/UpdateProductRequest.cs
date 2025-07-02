using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.WebApi.Models
{
	public class UpdateProductRequest
	{
        [Required(ErrorMessage = "Cafe ID zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçersiz Cafe ID")]
        public int CafeId { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ürün adı 2-100 karakter arasında olmalıdır")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Kategori zorunludur")]
        [StringLength(50, ErrorMessage = "Kategori maksimum 50 karakter olabilir")]
        public string Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz")]
        public int? StockQuantity { get; set; }

        [Url(ErrorMessage = "Geçersiz URL formatı")]
        [StringLength(500, ErrorMessage = "Resim URL'si maksimum 500 karakter olabilir")]
        public string ImageUrl { get; set; }
    }
}

