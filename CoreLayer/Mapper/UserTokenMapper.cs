using CoreLayer.DTOs.UserToken;
using DataLayer.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Mapper
{
    internal class UserTokenMapper
    {
        public static UesrTokenDto MapToDto(UserToken userToken)
        {

            return new UesrTokenDto
            {
                CreationDate = userToken.CreationDate,
                Device = userToken.Device,
                HashJwtToken = userToken.HashJwtToken,
                HashRefreshToken = userToken.HashRefreshToken,
                Id = userToken.Id,
                RefreshTokenExpireDate = userToken.RefreshTokenExpireDate,
                TokenExpireDate = userToken.RefreshTokenExpireDate,
                UserId = userToken.UserId,
            };
        }

        public static UserToken CreateUserTokenMapper(CreateUserTokenDto dto)
        {
            return new UserToken()
            {
                UserId = dto.UserId,
                TokenExpireDate = dto.TokenExpireDate,
                CreationDate = dto.CreationDate,
                Device = dto.Device,
                HashJwtToken = dto.HashJwtToken,
                HashRefreshToken = dto.HashRefreshToken,
                RefreshTokenExpireDate = dto.RefreshTokenExpireDate,
                
            };
        }
    }
}
