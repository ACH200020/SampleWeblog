using BlogWeb.Utilities;
using BlogWeb.Utilities.PDFCreater;
using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace BlogWeb.Areas.Admin.Pages.Post
{
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class AddModel : BaseController
    {

        #region Services
        private readonly IPostService _postService;
        private readonly IReportService _report;
        public AddModel(IPostService postService, IReportService report = null)
        {
            _postService = postService;
            _report = report;
        }

        #endregion

        #region Models
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [UIHint("Ckeditor4")]
        public string Description { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Slug { get; set; }

        [Display(Name = "عکس پست")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "عنوان عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageAlt { get; set; }

        #endregion
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.GetUserId();
            var writer = User.GetUserFullName();
            var result = _postService.CreatePost(new CreatePostDto()
            {
                CreationDate = DateTime.Now,
                Description = Description,
                ImagePost = ImageFile,
                Slug = Slug,
                Title = Title,
                UserId = userId,
                ImageAlt = ImageAlt

            });
            var post = _postService.GetPostBySlug(Slug);

            string fullPath = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToString() +
                Directories.GetPostImage(post.ImagePost);

            result = _report.GeneratePdfReport(new PDFObject()
            {
                CreationDate = DateTime.Now,
                Description = Description,
                ImageName = Directories.GetPostImage(post.ImagePost),
                Slug = Slug,
                Title = Title,
                Writer = writer
            }, fullPath);
            if (result.Status != OperationResultStatus.Success)
            {
                ModelState.AddModelError(nameof(Slug), result.Message);
                return Page();
            }
            

            return Redirect("Index");

        }
    }
}
