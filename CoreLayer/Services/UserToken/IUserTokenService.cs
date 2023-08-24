using CoreLayer.DTOs.UserToken;
using CoreLayer.Mapper;
using CoreLayer.Utilities;
using CoreLayer.Utilities.SecurityUtil;
using DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.UserToken
{
    public interface IUserTokenService
    {
        OperationResult CreateToken(CreateUserTokenDto command);
        OperationResult CheckExpireToken(string token, string refreshToken, int userId);

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

        public OperationResult CheckExpireToken(string token,string refreshToken, int userId)
        {

            var userToken = _context.UserTokens
                .Include(u=>u.User)
                .FirstOrDefault(x => x.UserId == userId);

            if (userToken == null)
            {
                return OperationResult.NotFound();
            }

            if (userToken.User.IsActive == false)
            {
                DeleteToken(userId);
                return OperationResult.NotFound();
            }

            if (userToken.TokenExpireDate > DateTime.Now)
                return OperationResult.Error("token is valid");

            if (Sha256Hasher.Hash(token) != userToken.HashJwtToken)
            {
                DeleteToken(userId);
                return OperationResult.NotFound();
            }

            if(Sha256Hasher.Hash(refreshToken) != userToken.HashRefreshToken)
            {
                DeleteToken(userId);
                return OperationResult.NotFound();
            }

            if (userToken.TokenExpireDate < DateTime.Now)
                if (userToken.RefreshTokenExpireDate > DateTime.Now)
                    return OperationResult.Success();

            return OperationResult.Error();
        }

        public OperationResult CreateToken(CreateUserTokenDto command)
        {
            var oldToken = _context.UserTokens.FirstOrDefault(f => f.UserId == command.UserId);

            if (oldToken != null && command.Device != oldToken.Device)
            {
                /*DeleteToken(oldToken.UserId);
                var newTokenn = UserTokenMapper.CreateUserTokenMapper(command);
                _context.UserTokens.Add(newTokenn);
                _context.SaveChanges();*/
                
                return OperationResult.Error("فقط یک دستگاه به طور همزمان قابل استفاده است");
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
