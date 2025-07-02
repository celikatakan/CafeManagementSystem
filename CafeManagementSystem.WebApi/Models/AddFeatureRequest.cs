using System;
using System.ComponentModel.DataAnnotations;

namespace CafeManagementSystem.WebApi.Models
{
	public class AddFeatureRequest
	{
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }
    }
}

