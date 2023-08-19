using CoreLayer.DTOs.UserRole;
using CoreLayer.Mapper;
using CoreLayer.Utilities;
using DataLayer.Context;
using DataLayer.Entities.Users;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.UserRole
{
    public interface IUserRoleService
    {
        int CreateUserRole(CreateUserRoleDto command);
        OperationResult EditUserRole(EditUserRoleDto command);

        UserRoleDto GetRole(int id);
    }

    public class UserRoleService : IUserRoleService
    {
        private readonly BlogContext _context;

        public UserRoleService(BlogContext context)
        {
            _context = context;
        }

        public int CreateUserRole(CreateUserRoleDto command)
        {

            var userRole = UserRoleMapper.CreateUserRole(command);
            _context.UserRoles.Add(userRole);
            _context.SaveChanges();

            return userRole.Id;

        }

        public OperationResult EditUserRole(EditUserRoleDto command)
        {
            var userRole = _context.UserRoles.FirstOrDefault(u=>u.Id ==command.Id);
            if (userRole == null)
            {
                return OperationResult.NotFound();
            }
            var newUserRole= UserRoleMapper.EditUserRoleMapper(command,userRole);
            _context.UserRoles.Update(newUserRole);
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public UserRoleDto GetRole(int id)
        {
            var userRole = _context.UserRoles.FirstOrDefault(u => u.Id == id);

            return UserRoleMapper.MapToDto(userRole);
        }
    }
}
