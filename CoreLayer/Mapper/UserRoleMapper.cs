using CoreLayer.DTOs.UserRole;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Mapper
{
    internal class UserRoleMapper
    {
        public static UserRole EditUserRoleMapper(EditUserRoleDto dto, UserRole role)
        {
            role.CreationDate = DateTime.Now;
            role.Writer = dto.Writer;
            role.Admin = dto.Admin;
            role.Comment = dto.Comment;
            role.Downloader = dto.Downloader;
            role.User = dto.User;
            return role;
        }

        public static UserRoleDto MapToDto(UserRole role)
        {
            if(role == null)
                return new UserRoleDto();

            return new UserRoleDto()
            {
                Admin = role.Admin,
                Comment = role.Comment,
                Downloader = role.Downloader,
                CreationDate = role.CreationDate,
                User = role.User,
                Id = role.Id,
                Writer = role.Writer
            };
        }

        public static UserRole CreateUserRole(CreateUserRoleDto dto)
        {
            return new UserRole()
            {
                Admin = dto.Admin,
                Comment = dto.Comment,
                Downloader = dto.Downloader,
                CreationDate = DateTime.Now,
                Writer = dto.Writer,
                User = dto.User
            };
        }
    }
}
