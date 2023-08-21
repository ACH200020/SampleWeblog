using CoreLayer.DTOs.Post;
using CoreLayer.Mapper;
using CoreLayer.Services.FileManagment;
using CoreLayer.Utilities;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Post
{
    public interface IPostService
    {
        OperationResult CreatePost(CreatePostDto command);
        OperationResult EditPost(EditPostDto command);
        OperationResult DeletePost(int id); 
        
        bool IsSlugExist(string slug);
        List<PostDto> GetAllPosts();
        PostDto GetPostById(int id);
        PostDto GetPostBySlug(string slug);
    }

    public class PostService : IPostService
    {
        private readonly BlogContext _context;
        private readonly IFileManager _fileManager;
        public PostService(BlogContext context, IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public OperationResult CreatePost(CreatePostDto command)
        {

            if (command.ImagePost == null)
                return OperationResult.Error();
            
            var post = PostMapper.CreatePostMap(command);
            if (IsSlugExist(post.Slug))
                return OperationResult.Error("slug تکراری است");

            post.ImagePost = _fileManager.SaveImageAndReturnImageName(command.ImagePost, Directories.PostImage);

            _context.Posts.Add(post);
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public OperationResult DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if(post == null)
                return OperationResult.NotFound();

            _context.Posts.Remove(post);
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public OperationResult EditPost(EditPostDto command)
        {
            var post = _context.Posts.FirstOrDefault(p=>p.Id== command.Id);
            if(post==null)
                return OperationResult.NotFound();

            var oldImage = post.ImagePost;
            var newSlug = post.Slug.ToSlug();

            if (post.Slug != newSlug)
                if (IsSlugExist(newSlug))
                    return OperationResult.Error("slug تکراری است");


            var newPost = PostMapper.EditPostMap(command, post);

            if (command.ImagePost != null)
                post.ImagePost =
                    _fileManager.SaveImageAndReturnImageName(command.ImagePost, Directories.PostImage);

            _context.Posts.Update(newPost);
            _context.SaveChanges();

            if (command.ImagePost != null)
            {
                _fileManager.DeleteFile(oldImage, Directories.PostImage);
            }

            return OperationResult.Success();
        }

        public List<PostDto> GetAllPosts()
        {
            return _context.Posts
                .Include(p=>p.User)
                .Select(post=>PostMapper.MapToDto(post))
                .ToList();
        }

        public PostDto GetPostById(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            if(post==null)
                return new PostDto();
            return PostMapper.MapToDto(post );
        }

        public PostDto GetPostBySlug(string slug)
        {
            var post = _context.Posts.FirstOrDefault(p=>p.Slug == slug);
            if(post==null) return new PostDto();
            return PostMapper.MapToDto(post);
        }

        public bool IsSlugExist(string slug)
        {
            return _context.Posts.Any(p => p.Slug == slug.ToSlug());
        }
    }
}
