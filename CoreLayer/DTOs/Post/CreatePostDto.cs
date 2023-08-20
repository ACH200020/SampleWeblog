using Microsoft.AspNetCore.Http;

namespace CoreLayer.DTOs.Post
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImagePost { get; set; }
        public string Slug { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string ImageAlt { get; set; }
    }
}
