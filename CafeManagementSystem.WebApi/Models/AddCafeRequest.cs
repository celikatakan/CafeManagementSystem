using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.WebApi.Models
{
	public class AddCafeRequest
	{
        [Required]
        public string Name { get; set; }
        public int? Stars { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public List<int> FeatureIds { get; set; }
    }
}

