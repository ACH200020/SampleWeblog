using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities
{
    public class Directories
    {
        public const string PostImage = "wwwroot/images/post";
        public const string AvatarImage = "wwwroot/images/avatar";

        public static string GetPostImage(string imageName)
            => $"{PostImage.Replace("wwwroot", "")}/{imageName}";

        public static string GetAvatarImage(string imageName)
            => $"{AvatarImage.Replace("wwwroot", "")}/{imageName}";

    }
}
