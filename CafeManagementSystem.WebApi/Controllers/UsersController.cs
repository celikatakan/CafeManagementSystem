using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CafeManagementSystem.Business.Operations.User;
using CafeManagementSystem.Business.Operations.User.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CafeManagementSystem.Business.Operations.Cafe;

namespace CafeManagementSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ICafeLikeService _cafeLikeService;

        public UsersController(IUserService userService, ICafeLikeService cafeLikeService)
        {
            _userService = userService;
            _cafeLikeService = cafeLikeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddUserDto dto)
        {
            return Ok(await _userService.UpdateUserAsync(id, dto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("{id}/likedcafes")]
        [Authorize]
        public async Task<IActionResult> GetLikedCafes(int id)
        {
            var cafes = await _cafeLikeService.GetLikedCafesByUserAsync(id);
            return Ok(cafes);
        }
    }
} 