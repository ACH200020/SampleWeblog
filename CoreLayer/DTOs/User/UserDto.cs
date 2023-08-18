using DataLayer.Entities;
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
        public string Name { get; set; }
        public string Family { get; set; }
        public UserRole UserRole { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string ImageAvatarName { get; set; }
    }

    public enum UserRole
    {
        PanelAdmin,
        EditProfile,
        ChangePassword,
        Writer,
        Downloader,
        User
    }

    public class LoginDto
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
