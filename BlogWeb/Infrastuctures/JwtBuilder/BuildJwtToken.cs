using AngleSharp;
using CoreLayer.DTOs.User;
using DataLayer.Entities.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace BlogWeb.Infrastuctures.JwtBuilder
{
    public class BuildJwtToken
    {

        public static string BuildToken(List<string> roles, UserDto user, IConfiguration _configuration,string? refreshToken)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.MobilePhone,user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName)

            };

            for (int i = 0; i < roles.Count; i++)
            {
                claims.Add(new Claim(ClaimTypes.Role, roles[i]));
            }

            var secretKey = 
                refreshToken == null ?
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SignInKey"]))
                : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshToken));

            var credential = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
            claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credential);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
