using CoreLayer.DTOs.UserRole;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs.User
{
    public class UserDto : BaseDTO
    {
        public string FullName { get; set; }
        public int UserRoleId { get; set; }
        public UserRoleDto UserRole { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageAvatarName { get; set; }
        public bool IsActive { get; set; }

    }
}
