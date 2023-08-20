using CoreLayer.DTOs.User;

namespace CoreLayer.DTOs.UserRole
{
    public class UserRoleDto
    {
        public DateTime CreationDate { get; set; }
        public int Id { get; set; }
        public bool Admin { get; set; }
        public bool User { get; set; }
        public bool Writer { get; set; }
        public bool Downloader { get; set; }
        public bool Comment { get; set; }
    }
}
