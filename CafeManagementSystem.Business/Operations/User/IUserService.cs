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
        //Task<ServiceMessage<UserEntity>> GetUserById(int userId);
    }
}

