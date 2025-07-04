using System;
using CafeManagementSystem.Business.DataProtection;
using CafeManagementSystem.Business.Operations.User.Dtos;
using CafeManagementSystem.Business.Types;
using CafeManagementSystem.Data.Entities;
using CafeManagementSystem.Data.Enums;
using CafeManagementSystem.Data.Repositories;
using CafeManagementSystem.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace CafeManagementSystem.Business.Operations.User
{
	public class UserManager : IUserService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
        {

            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }
        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email adresi mevcut."
                };
            }

            var userEntity = new UserEntity
            {
                Email = user.Email,
                Password = _protector.Protect(user.Password),
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                UserType = UserType.Customer
            };

            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Kullanıcı kaydı sırasında bir hata oluştu.");
            }
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı başarıyla kaydedildi."
            };
        }

        //public async Task<ServiceMessage<UserEntity>> GetUserById(int userId)
        //{
        //    var user = await _userRepository.Get(x => x.Id == userId); 
        //    if (user == null)
        //        return new ServiceMessage<UserEntity> { IsSucceed = false, Message = "Kullanıcı bulunamadı" };

        //    return new ServiceMessage<UserEntity> { IsSucceed = true, Data = user };
        //}

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity == null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }

            var unprotectedPassword = _protector.Unprotect(userEntity.Password);

            if (unprotectedPassword == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Id = userEntity.Id,
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType,
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }
        }

        public async Task<List<UserInfoDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAll().ToListAsync();
            return users.Select(u => new UserInfoDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserType = u.UserType
            }).ToList();
        }

        public async Task<UserInfoDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetAll(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType
            };
        }

        public async Task<UserInfoDto> CreateUserAsync(AddUserDto dto)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == dto.Email.ToLower());
            if (hasMail.Any())
                throw new InvalidOperationException("Email adresi mevcut.");

            var userEntity = new UserEntity
            {
                Email = dto.Email,
                Password = _protector.Protect(dto.Password),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                UserType = UserType.Customer
            };

            _userRepository.Add(userEntity);
            await _unitOfWork.SaveChangesAsync();

            return new UserInfoDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                UserType = userEntity.UserType
            };
        }

        public async Task<UserInfoDto> UpdateUserAsync(int id, AddUserDto dto)
        {
            var user = await _userRepository.GetAll(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            // Email kontrolü (kendi email'i hariç)
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == dto.Email.ToLower() && x.Id != id);
            if (hasMail.Any())
                throw new InvalidOperationException("Email adresi mevcut.");

            user.Email = dto.Email;
            user.Password = _protector.Protect(dto.Password);
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.BirthDate = dto.BirthDate;

            await _unitOfWork.SaveChangesAsync();

            return new UserInfoDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserType = user.UserType
            };
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetAll(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found");

            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

