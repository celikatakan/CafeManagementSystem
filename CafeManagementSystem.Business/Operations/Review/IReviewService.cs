using System;
using CafeManagementSystem.Business.Operations.Review.Dtos;
using CafeManagementSystem.Business.Types;

namespace CafeManagementSystem.Business.Operations.Review
{
	public interface IReviewService
	{
        Task<ServiceMessage<ReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<ServiceMessage<List<ReviewDto>>> GetReviewsByCafeIdAsync(int cafeId);
    }
}

