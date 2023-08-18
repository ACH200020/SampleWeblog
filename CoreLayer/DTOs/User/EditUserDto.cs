using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.User
{
    public class EditUserDto 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public IFormFile Avatar { get; set; }
        public string Password { get; set; }
    }
}
