using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly IPostService _postService;

        public IndexModel(IPostService postService)
        {
            _postService = postService;
        }

        public List<PostDto> Posts{ get; set; }
        public IActionResult OnGet()
        {
            Posts = _postService.GetAllPosts();
            return Page();
        }
    }
}