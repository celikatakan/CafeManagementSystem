using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;

namespace CafeManagementSystem.Business.Operations.Cafe
{
    public interface ICafeLikeService
    {
        Task LikeCafeAsync(int cafeId, int userId);
        Task UnlikeCafeAsync(int cafeId, int userId);
        Task<bool> HasUserLikedCafeAsync(int cafeId, int userId);
        Task<int> GetCafeLikeCountAsync(int cafeId);
        Task<List<CafeDto>> GetLikedCafesByUserAsync(int userId);
    }
} 