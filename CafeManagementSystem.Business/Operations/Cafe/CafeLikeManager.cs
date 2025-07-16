using System.Threading.Tasks;
using CafeManagementSystem.Data.UnitOfWork;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Repositories;
using System.Linq;
using System.Collections.Generic;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;

namespace CafeManagementSystem.Business.Operations.Cafe
{
    public class CafeLikeManager : ICafeLikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CafeLikeEntity> _cafeLikeRepository;
        private readonly IRepository<CafeEntity> _cafeRepository;

        public CafeLikeManager(IUnitOfWork unitOfWork, IRepository<CafeLikeEntity> cafeLikeRepository, IRepository<CafeEntity> cafeRepository)
        {
            _unitOfWork = unitOfWork;
            _cafeLikeRepository = cafeLikeRepository;
            _cafeRepository = cafeRepository;
        }

        public async Task LikeCafeAsync(int cafeId, int userId)
        {
            var like = _cafeLikeRepository.GetAll()
                .FirstOrDefault(cl => cl.CafeId == cafeId && cl.UserId == userId);

            if (like == null)
            {
                var newLike = new CafeLikeEntity
                {
                    CafeId = cafeId,
                    UserId = userId
                };
                _cafeLikeRepository.Add(newLike);
                await _unitOfWork.SaveChangesAsync();
            }
          
        }

        public async Task UnlikeCafeAsync(int cafeId, int userId)
        {
            var like = _cafeLikeRepository.GetAll()
                .FirstOrDefault(cl => cl.CafeId == cafeId && cl.UserId == userId);

            if (like != null)
            {
                _cafeLikeRepository.Delete(like, false);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public Task<bool> HasUserLikedCafeAsync(int cafeId, int userId)
        {
            return Task.FromResult(_cafeLikeRepository.GetAll()
                .Any(cl => cl.CafeId == cafeId && cl.UserId == userId));
        }

        public Task<int> GetCafeLikeCountAsync(int cafeId)
        {
            return Task.FromResult(_cafeLikeRepository.GetAll()
                .Count(cl => cl.CafeId == cafeId));
        }

        public async Task<List<CafeDto>> GetLikedCafesByUserAsync(int userId)
        {
            var likedCafeIds = _cafeLikeRepository.GetAll()
                .Where(cl => cl.UserId == userId)
                .Select(cl => cl.CafeId)
                .ToList();

            var cafes = _cafeRepository.GetAll()
                .Where(c => likedCafeIds.Contains(c.Id))
                .Select(x => new CafeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Stars = x.Stars,
                    Location = x.Location,
                    Features = x.CafeFeatures.Select(f => new CafeFeatureDto
                    {
                        Id = f.Id,
                        Title = f.Feature.Title
                    }).ToList()
                }).ToList();

            return await Task.FromResult(cafes);
        }
    }
} 