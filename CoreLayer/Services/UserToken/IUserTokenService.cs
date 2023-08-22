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

namespace CoreLayer.Services.UserToken
{
    public interface IUserTokenService
    {
        OperationResult CreateToken(CreateUserTokenDto command);
        OperationResult CheckExpireTokenAndReCreate(string token, string refreshToken, CreateUserTokenDto command);

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

        public OperationResult CheckExpireTokenAndReCreate(string token,string refreshToken, CreateUserTokenDto command)
        {
            var userToken = _context.UserTokens.FirstOrDefault(x => x.UserId == command.UserId);
            if(userToken == null)
                return OperationResult.NotFound();

            if(userToken.TokenExpireDate> userToken.RefreshTokenExpireDate)
                return OperationResult.Error();

            if (Sha256Hasher.Hash(token) != userToken.HashJwtToken)
                return OperationResult.Error();

            if(Sha256Hasher.Hash(refreshToken) != userToken.HashRefreshToken)
                return OperationResult.Error();

            if(userToken.TokenExpireDate < userToken.RefreshTokenExpireDate)
                if (userToken.RefreshTokenExpireDate > DateTime.Now)
                    return OperationResult.Error();
                else
                {
                    DeleteToken(command.UserId);
                    CreateToken(new CreateUserTokenDto()
                    {
                        HashJwtToken = Sha256Hasher.Hash(command.HashJwtToken),
                        RefreshTokenExpireDate = command.RefreshTokenExpireDate,
                        CreationDate = command.CreationDate,
                        Device = command.Device,
                        HashRefreshToken = Sha256Hasher.Hash(command.HashRefreshToken),
                        TokenExpireDate = command.TokenExpireDate,
                        UserId = command.UserId
                    });
                    return OperationResult.Success();
                }
            else
            {
                return OperationResult.Success();
            }



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
