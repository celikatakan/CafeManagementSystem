using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.Cafe;
using CafeManagementSystem.Business.Operations.Cafe.Dtos;
using CafeManagementSystem.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CafeManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CafesController : Controller
    {
        private readonly ICafeService _cafeService;
        private readonly ICafeLikeService _cafeLikeService;

        public CafesController(ICafeService cafeService, ICafeLikeService cafeLikeService)
        {
            _cafeService = cafeService;
            _cafeLikeService = cafeLikeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCafe(int id)
        {
            var cafe = await _cafeService.GetCafe(id);
            if (cafe != null)
                return Ok(cafe);
            else
                return NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetCafes()
        {
            var cafes = await _cafeService.GetCafes();

            return Ok(cafes);
        }
        [HttpPost]
        public async Task<IActionResult> AddCafe([FromBody] AddCafeRequest request)
        {
            var addCafeDto = new AddCafeDto
            {
                Name = request.Name,
                Stars = request.Stars,
                Location = request.Location,
                FeatureIds = request.FeatureIds
            };
            var result = await _cafeService.AddCafe(addCafeDto);

            if (result.IsSucceed)
                return Ok(result);
            else
                return BadRequest(result.Message);
        }
        [HttpPatch("{id}/stars")]
        public async Task<IActionResult> AddjustCafeStars(int id, int changeTo)
        {
            var result = await _cafeService.AddjustCafeStars(id, changeTo);

            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCafe(int id, [FromBody] UpdateCafeRequest request)
        {
            var updateCafeDto = new UpdateCafeDto
            {
                Id = id,
                Name = request.Name,
                Stars = request.Stars,
                Location = request.Location,
                FeatureIds = request.FeatureIds
            };

            var result = await _cafeService.UpdateCafe(updateCafeDto);
            if (!result.IsSucceed)
                return NotFound(result.Message);
            else
                return await GetCafe(id);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCafe(int id)
        {
            var result = await _cafeService.DeleteCafe(id);
            if (!result.IsSucceed)
                return BadRequest(result.Message);
            else
                return Ok(result);
        }

        [HttpPost("{id}/like")]
        [Authorize]
        public async Task<IActionResult> LikeCafe(int id)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Kullanıcı bulunamadı.");
            await _cafeLikeService.LikeCafeAsync(id, userId);
            return Ok();
        }

        [HttpDelete("{id}/like")]
        [Authorize]
        public async Task<IActionResult> UnlikeCafe(int id)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Kullanıcı bulunamadı.");
            await _cafeLikeService.UnlikeCafeAsync(id, userId);
            return Ok();
        }

        [HttpGet("{id}/likes/count")]
        [Authorize]
        public async Task<IActionResult> GetCafeLikeCount(int id)
        {
            var count = await _cafeLikeService.GetCafeLikeCountAsync(id);
            return Ok(new { count });
        }

        [HttpGet("{id}/likes/hasliked")]
        [Authorize]
        public async Task<IActionResult> HasUserLikedCafe(int id)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (userId == 0)
                return Unauthorized("Kullanıcı bulunamadı.");
            var liked = await _cafeLikeService.HasUserLikedCafeAsync(id, userId);
            return Ok(new { liked });
        }


    }
}

