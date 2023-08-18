using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Post
{
    public class EditPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImagePost { get; set; }
        public string Slug { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
