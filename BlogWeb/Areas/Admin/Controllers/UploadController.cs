
using CoreLayer.Services.FileManagment;
using CoreLayer.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.Areas.Admin.Controllers;

public class UploadController : Controller
{
    private readonly IFileManager _fileManager;
    public UploadController(IFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    [Route("/Upload/Article")]
    public IActionResult UploadArticleImage(IFormFile upload)
    {
        if (upload == null)
        {
            BadRequest();
        }

        var image = _fileManager.SaveImageAndReturnImageName(upload, Directories.PostContentImage);
        return Json(new { Uploaded = true, url = Directories.GetPostContentImage(image) });
    }
}
