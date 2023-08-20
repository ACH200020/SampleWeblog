using CoreLayer.DTOs.UserToken;
using CoreLayer.Mapper;
using CoreLayer.Utilities;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.UserToken
{
    public interface IUserTokenService
    {
        OperationResult CreateToken(CreateUserTokenDto command);

        UesrTokenDto GetTokenByUserId(int id);
        OperationResult DeleteToken(int id);
    }

    public class UserTokenService : IUserTokenService
    {
        private readonly BlogContext _context;

        public UserTokenService(BlogContext context)
        {
            _context = context;
        }

        public OperationResult CreateToken(CreateUserTokenDto command)
        {
            var oldToken = _context.UserTokens.FirstOrDefault(f => f.UserId == command.UserId);

            if (oldToken != null)
            {
                DeleteToken(oldToken.UserId);
                var newTokenn = UserTokenMapper.CreateUserTokenMapper(command);
                _context.UserTokens.Add(newTokenn);
                _context.SaveChanges();
                return OperationResult.Error("فقط یک دستگاه به طور همزمان قابل استفاده است به همین دلیل از دستگاه قبلی خارج شدید");
            }

            var newToken = UserTokenMapper.CreateUserTokenMapper(command);
            _context.UserTokens.Add(newToken);
            _context.SaveChanges();
            return OperationResult.Success();


        }

        public OperationResult DeleteToken(int userId)
        {
            var result = _context.UserTokens.FirstOrDefault(f => f.UserId == userId);
            _context.UserTokens.Remove(result);
            _context.SaveChanges();
            return OperationResult.Success();
        }

        public UesrTokenDto GetTokenByUserId(int userId)
        {
            var result = _context.UserTokens.FirstOrDefault(f => f.UserId == userId);
            return UserTokenMapper.MapToDto(result);
        }
    }
}
