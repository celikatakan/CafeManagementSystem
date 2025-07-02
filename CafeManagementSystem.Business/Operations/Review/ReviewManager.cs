using System;
using CafeManagementSystem.Business.Operations.Review.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CafeManagementSystem.Business.Operations.Review
{

        public class ReviewManager : IReviewService
        {
            private readonly IRepository<ReviewEntity> _reviewRepository;
            private readonly IRepository<CafeEntity> _cafeRepository;
            private readonly IRepository<UserEntity> _userRepository;
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<ReviewManager> _logger;

            public ReviewManager(
                IRepository<ReviewEntity> reviewRepository,
                IRepository<CafeEntity> cafeRepository,
                IRepository<UserEntity> userRepository,
                IUnitOfWork unitOfWork,
                ILogger<ReviewManager> logger)
            {
                _reviewRepository = reviewRepository;
                _cafeRepository = cafeRepository;
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
                _logger = logger;
            }

            public async Task<ServiceMessage<ReviewDto>> CreateReviewAsync(CreateReviewDto createReviewDto)
            {
                try
                {
                    // Cafe'nin var olup olmadığını kontrol et
                    var cafe = await _cafeRepository.GetByIdAsync(createReviewDto.CafeId);
                    if (cafe == null)
                    {
                        return new ServiceMessage<ReviewDto>
                        {
                            IsSucceed = false,
                            Message = "Cafe bulunamadı."
                        };
                    }

                    // Kullanıcının var olup olmadığını kontrol et
                    var user = await _userRepository.GetByIdAsync(createReviewDto.UserId);
                    if (user == null)
                    {
                        return new ServiceMessage<ReviewDto>
                        {
                            IsSucceed = false,
                            Message = "Kullanıcı bulunamadı."
                        };
                    }

                    // Kullanıcının bu cafe için daha önce yorum yapıp yapmadığını kontrol et
                    var existingReview = await _reviewRepository.GetAllAsync(r => r.CafeId == createReviewDto.CafeId && r.UserId == createReviewDto.UserId);
                    if (existingReview.Any())
                    {
                        return new ServiceMessage<ReviewDto>
                        {
                            IsSucceed = false,
                            Message = "Bu cafe için zaten yorum yapmışsınız."
                        };
                    }

                    var review = new ReviewEntity
                    {
                        CafeId = createReviewDto.CafeId,
                        UserId = createReviewDto.UserId,
                        Rating = createReviewDto.Rating,
                        Comment = createReviewDto.Comment,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _reviewRepository.AddAsync(review);
                    await _unitOfWork.SaveChangesAsync();

                    var reviewDto = new ReviewDto(
                        review.Id,
                        review.CafeId,
                        review.UserId,
                        $"{user.FirstName} {user.LastName}",
                        review.Rating,
                        review.Comment ?? string.Empty,
                        review.CreatedDate
                    );

                    return new ServiceMessage<ReviewDto>
                    {
                        IsSucceed = true,
                        Message = "Yorum başarıyla eklendi.",
                        Data = reviewDto
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Yorum eklenirken hata oluştu.");
                    return new ServiceMessage<ReviewDto>
                    {
                        IsSucceed = false,
                        Message = "Yorum eklenirken bir hata oluştu."
                    };
                }
            }

            public async Task<ServiceMessage<List<ReviewDto>>> GetReviewsByCafeIdAsync(int cafeId)
            {
                try
                {
                    var reviews = await _reviewRepository.GetAllAsync(
                        r => r.CafeId == cafeId,
                        include: q => q.Include(r => r.User)
                    );

                    var reviewDtos = reviews.Select(r => new ReviewDto(
                        r.Id,
                        r.CafeId,
                        r.UserId,
                        $"{r.User.FirstName} {r.User.LastName}",
                        r.Rating,
                        r.Comment ?? string.Empty,
                        r.CreatedDate
                    )).OrderByDescending(r => r.CreatedDate).ToList();

                    return new ServiceMessage<List<ReviewDto>>
                    {
                        IsSucceed = true,
                        Message = "Yorumlar başarıyla getirildi.",
                        Data = reviewDtos
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Yorumlar getirilirken hata oluştu.");
                    return new ServiceMessage<List<ReviewDto>>
                    {
                        IsSucceed = false,
                        Message = "Yorumlar getirilirken bir hata oluştu."
                    };
                }
            }
        }
    }


