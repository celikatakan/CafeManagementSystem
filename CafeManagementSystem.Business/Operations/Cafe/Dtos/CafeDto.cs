using System;
namespace CafeManagementSystem.Business.Operations.Cafe.Dtos
{
	public class CafeDto
	{
		public int Id { get; set; }
        public string Name { get; set; }
        public int? Stars { get; set; }
        public string Location { get; set; }
        public List<CafeFeatureDto> Features { get; set; }
    }
}

