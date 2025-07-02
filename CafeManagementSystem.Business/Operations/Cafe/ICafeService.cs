using System;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.Business.Types;

namespace CafeManagementSystem.Business.Operations.Cafe
{
	public interface ICafeService
	{
		Task<CafeDto> GetCafe(int id);
		Task<List<CafeDto>> GetCafes();
		Task<ServiceMessage> AddCafe(AddCafeDto cafe);
		Task<ServiceMessage> AddjustCafeStars(int id, int changeTo);
		Task<ServiceMessage> UpdateCafe(UpdateCafeDto cafe);
		Task<ServiceMessage> DeleteCafe(int id);
	}
}

