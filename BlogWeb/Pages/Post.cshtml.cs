using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Pages
{
    public class PostModel : PageModel
    {
        private readonly IPostService _postService;

        public PostModel(IPostService postService)
        {
            _postService = postService;
        }

        public PostDto Post{ get; set; }
        public IActionResult OnGet(string slug)
        {
            Post = _postService.GetPostBySlug(slug);
            return Page();
        }
    }
}
