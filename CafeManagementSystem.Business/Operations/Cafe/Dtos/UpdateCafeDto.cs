using System;
namespace CafeManagementSystem.Business.Operations.Cafe.Dtos
{
	public class UpdateCafeDto
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Stars { get; set; }
        public string Location { get; set; }
        public List<int> FeatureIds { get; set; }
    }
}

