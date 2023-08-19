namespace CoreLayer.DTOs.UserRole
{
    public class EditUserRoleDto
    {
        public int Id { get; set; }
        public bool Admin { get; set; }
        public bool User { get; set; }
        public bool Writer { get; set; }
        public bool Downloader { get; set; }
        public bool Comment { get; set; }
    }
}
