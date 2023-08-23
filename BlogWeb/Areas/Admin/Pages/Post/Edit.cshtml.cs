using BlogWeb.Utilities;
using BlogWeb.Utilities.PDFCreater;
using CoreLayer.DTOs.Post;
using CoreLayer.Services.Post;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Areas.Admin.Pages.Post
{
    [ValidateAntiForgeryToken]
    [BindProperties]
    public class EditModel : BaseController
    {
        #region Services
        private readonly IPostService _postService;
        private readonly IReportService _reportService;
        public EditModel(IPostService postService, IReportService reportService)
        {
            _postService = postService;
            _reportService = reportService;
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
        public IFormFile ImagePost { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "عنوان عکس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageAlt { get; set; }

        private static string oldSlug;
        #endregion

        public void OnGet(int id)
        {
            var post = _postService.GetPostById(id);

            Title = post.Title;
            Description = post.Description;
            Slug = post.Slug;
            ImageAlt = post.ImageAlt;
            oldSlug = post.Slug;
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
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PostPdf");

            string writer = User.GetUserFullName();

            var post = _postService.GetPostById(id);

            var fullPath = HttpContext.Request.Scheme+"://"+HttpContext.Request.Host.ToString()+
                 Directories.GetPostImage(post.ImagePost);

            _reportService.DeletePdf($"{oldSlug}.pdf", path);
            _reportService.GeneratePdfReport(new PDFObject
            {
                CreationDate = DateTime.Now,
                Description = Description,
                Slug = Slug,
                ImageName = Directories.GetPostImage(post.ImagePost),
                Title = Title,
                Writer = writer
            },fullPath);


           

            return RedirectToPage("Index");
        }
    }
}
