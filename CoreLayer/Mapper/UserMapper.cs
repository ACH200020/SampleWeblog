using Common.Application.SecurityUtil;
using CoreLayer.DTOs.User;
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
                Family = user.Family,
                Id = user.Id,
                ImageAvatarName = user.ImageAvatarName,
                Name = user.Name,
                NationalCode = user.NationalCode,
                PhoneNumber = user.PhoneNumber,
                UserRole = (DTOs.User.UserRole)user.UserRole,
            };
        }

        internal static User AddUserMap(AddUserDto dto)
        {
            return new User()
            {
                CreationDate = dto.CreationDate,
                Family = dto.Family,
                ImageAvatarName = "R.png",
                Name = dto.Name,
                NationalCode = dto.NationalCode,
                PhoneNumber = dto.PhoneNumber,
                UserRole = DataLayer.Entities.Users.UserRole.User,
                Password = Sha256Hasher.Hash(dto.Password),
            };
        }

        internal static User EditUserMap(EditUserDto dto, User user)
        {
            user.Id = dto.Id;
            user.CreationDate = DateTime.Now;
            user.Family = dto.Family;
            user.Name = dto.Name;
            return user;
        }
    }
}
