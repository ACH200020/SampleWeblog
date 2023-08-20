using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWeb.Areas.Admin.Pages.Post
{
    public class IndexModel : PageModel
    {
        #region Services
        private readonly IPostService _postService;

        public IndexModel(IPostService postService)
        {
            _postService = postService;
        }
        #endregion

        #region Models

        public List<PostDto>? Posts { get; set; }

        #endregion

        public IActionResult OnGet()
        {
            Posts = _postService.GetAllPosts();
            return Page();
            
        }

        public IActionResult OnPost(int id)
        {
            _postService.DeletePost(id);
            Posts = _postService.GetAllPosts();

            return Page();
        }
    }
}
