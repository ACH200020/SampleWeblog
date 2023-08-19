using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs.UserRole
{
    public class CreateUserRoleDto
    {
        public bool Admin { get; set; }
        public bool User { get; set; }
        public bool Writer { get; set; }
        public bool Downloader { get; set; }
        public bool Comment { get; set; }
    }
}
