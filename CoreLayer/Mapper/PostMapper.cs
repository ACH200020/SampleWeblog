using CoreLayer.DTOs.Post;
using CoreLayer.Utilities;
using DataLayer.Entities.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Mapper
{
    internal class PostMapper
    {
        internal static PostDto MapToDto(Post post)
        {
            if (post == null)
                return new PostDto();

            return new PostDto()
            {
                Id = post.Id,
                CreationDate = post.CreationDate,
                Description = post.Description,
                ImagePost = post.ImagePost,
                Slug = post.Slug,
                Title = post.Title,
                User = post.User == null ? null : UserMapper.MapToDto(post.User),
                UserId = post.UserId,
                ImageAlt = post.ImageAlt,

            };
        }

        internal static Post CreatePostMap(CreatePostDto dto)
        {

            return new Post()
            {
                UserId = dto.UserId,
                CreationDate = dto.CreationDate,
                Description = dto.Description,
                Slug = dto.Slug.ToSlug(),
                Title = dto.Title,
                ImageAlt= dto.ImageAlt,
            };
        }

        internal static Post EditPostMap(EditPostDto dto, Post post)
        {
            post.Id = dto.Id;
            post.CreationDate = DateTime.Now;
            post.Description = dto.Description;
            post.Slug = dto.Slug.ToSlug();
            post.Title = dto.Title;
            post.ImageAlt = dto.ImageAlt;
            return post;
        }
    }
}
