using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.FileManagment
{
    public interface IFileManager
    {
        string SaveImageAndReturnImageName(IFormFile file, string savePath);
        string SaveFileAndReturnName(IFormFile file, string savePath);
        void DeleteFile(string fileName, string path);
    }
}
