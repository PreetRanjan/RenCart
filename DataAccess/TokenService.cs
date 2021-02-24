using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RenCart.API.DataAccess
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;
        private readonly AppDbContext db;

        public TokenService(IConfiguration config, AppDbContext db)
        {
            this.config = config;
            this.db = db;
        }
        public async Task<string> GenerateToken(AppUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha384Signature);

            var roles = await db.AppUserRoles.Where(x => x.AppUserId == user.Id).Select(x => x.Role).ToListAsync();
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString(CultureInfo.InvariantCulture)));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString(CultureInfo.InvariantCulture)));
            }

            var token = new JwtSecurityToken(
                issuer: config["AppSettings:Issuer"],
                audience: config["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }
    }
}
