using CoreLayer.DTOs.User;
using CoreLayer.Mapper;
using CoreLayer.Services.FileManagment;
using CoreLayer.Utilities;
using DataLayer.Context;
using Microsoft.EntityFrameworkCore;
using CoreLayer.Utilities.SecurityUtil;

namespace CoreLayer.Services.User
{
    public interface IUserService
    {
        OperationResult RegisterUser(RegisterUserDto command);
        OperationResult EditUser(EditUserDto command);

        UserDto? Login(LoginDto command);

        List<UserDto> GetUsers();
    }

    public class UserService : IUserService
    {
        private readonly BlogContext _context;
        private readonly IFileManager _fileManager;
        public UserService(BlogContext context,IFileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public OperationResult RegisterUser(RegisterUserDto command)
        {
            bool exists = _context.Users.Any(u=>u.PhoneNumber == command.PhoneNumber);
            if (exists)
            {
                return OperationResult.Error("شماره تماس تکراری است");
            }

            var user = UserMapper.AddUserMap(command);
            _context.Users.Add(user);
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public OperationResult EditUser(EditUserDto command)
        {
           
            var user = _context.Users.FirstOrDefault(u=>u.Id == command.Id);
            if(user == null)
                return OperationResult.NotFound();

            var oldImage = user.ImageAvatarName;

            var newUser = UserMapper.EditUserMap(command, user);

            if (command.Avatar != null)
                user.ImageAvatarName =
                    _fileManager.SaveImageAndReturnImageName(command.Avatar, Directories.AvatarImage);

            _context.Users.Update(newUser);
            _context.SaveChanges();

            if (command.Avatar != null && user.ImageAvatarName != "R.png")
            {
                _fileManager.DeleteFile(oldImage, Directories.AvatarImage);
            }

            return OperationResult.Success();

        }

        public List<UserDto> GetUsers()
        {
            return _context.Users
                .Include(u=>u.UserRole)
                .Select(user=>UserMapper.MapToDto(user))
                .ToList();
        }

        public UserDto? Login(LoginDto command)
        {
            var login = _context.Users.FirstOrDefault(u=>u.PhoneNumber == command.PhoneNumber && u.Password == Sha256Hasher.Hash(command.Password));

            if (login == null)
                return null;

            return UserMapper.MapToDto(login);

        }
    }
}
