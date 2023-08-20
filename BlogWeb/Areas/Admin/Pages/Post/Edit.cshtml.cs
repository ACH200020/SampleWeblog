using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Areas.Admin.Pages.Post
{
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class EditModel : PageModel
    {
        #region Services
        private readonly IPostService _postService;

        public EditModel(IPostService postService)
        {
            _postService = postService;
        }
        #endregion

        #region Models
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Slug { get; set; }

        [Display(Name = "عکس پست")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ImagePost { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "عنوان عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageAlt { get; set; }


        #endregion

        public void OnGet(int id)
        {
            var post = _postService.GetPostById(id);

            Title = post.Title;
            Description = post.Description;
            Slug = post.Slug;
            ImageAlt = post.ImageAlt;
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = _postService.EditPost(new EditPostDto()
            {
                Id = id,
                Title = Title,
                Description = Description,
                Slug = Slug,
                ImagePost = ImagePost,
                ImageAlt = ImageAlt,
                CreationDate = DateTime.Now,
            });
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(Slug), result.Message);
                return Page();
            }

            return RedirectToPage("Index");
        }
    }
}
