using System.Security.Claims;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Review;
using CafeManagementSystem.Business.Operations.Review.Dtos;
using CafeManagementSystem.Business.Operations.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CafeManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;

        public ReviewsController(IReviewService reviewService, IUserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        [HttpGet("cafe/{cafeId}")]
        public async Task<IActionResult> GetReviews(int cafeId)
        {
            var result = await _reviewService.GetReviewsByCafeIdAsync(cafeId);
            if (!result.IsSucceed)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reviewService.CreateReviewAsync(model);
            return result.IsSucceed ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
    
}

