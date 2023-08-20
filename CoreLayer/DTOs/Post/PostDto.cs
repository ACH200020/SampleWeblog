using CoreLayer.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DTOs.Post
{
    public class PostDto : BaseDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePost { get; set; }
        public string Slug { get; set; }
        public UserDto User { get; set; }
        public string ImageAlt { get; set; }

    }
}
