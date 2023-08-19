using CoreLayer.DTOs.User;
using CoreLayer.Services.UserRole;
using CoreLayer.Utilities.SecurityUtil;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Mapper
{
    internal class UserMapper
    {
        
        internal static UserDto MapToDto(User user)
        {
            return new UserDto()
            {
                CreationDate = user.CreationDate,
                Id = user.Id,
                ImageAvatarName = user.ImageAvatarName,
                FullName = user.FullName,
                NationalCode = user.NationalCode,
                PhoneNumber = user.PhoneNumber,
                UserRoleId = user.UserRoleId,
                UserRole = UserRoleMapper.MapToDto(user.UserRole)
            };
        }

        internal static User AddUserMap(RegisterUserDto dto)
        {
            return new User()
            {
                CreationDate = dto.CreationDate,
                ImageAvatarName = "R.png",
                FullName = dto.Name + " " + dto.Family,
                NationalCode = dto.NationalCode,
                PhoneNumber = dto.PhoneNumber,
                UserRoleId = dto.UserRoleId,
                Password = Sha256Hasher.Hash(dto.Password),
            };
        }

        internal static User EditUserMap(EditUserDto dto, User user)
        {
            user.Id = dto.Id;
            user.CreationDate = DateTime.Now;
            
            return user;
        }
    }
}
