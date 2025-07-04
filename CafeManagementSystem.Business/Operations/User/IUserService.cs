using System;
using CafeManagementSystem.Business.Operations.User.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;

namespace CafeManagementSystem.Business.Operations.User
{
	public interface IUserService
	{
        Task<ServiceMessage> AddUser(AddUserDto user);
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
        Task<List<UserInfoDto>> GetAllUsersAsync();
        Task<UserInfoDto> GetUserByIdAsync(int id);
        Task<UserInfoDto> CreateUserAsync(AddUserDto dto);
        Task<UserInfoDto> UpdateUserAsync(int id, AddUserDto dto);
        Task DeleteUserAsync(int id);
    }
}

