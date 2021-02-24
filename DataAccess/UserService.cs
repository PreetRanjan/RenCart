using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RenCart.API.Dtos;
using RenCart.API.Interfaces;
using RenCart.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RenCart.API.DataAccess
{
    public class UserService : IUserService
    {
        private readonly IConfiguration config;

        private readonly AppDbContext db;
        private readonly ITokenService tokenService;

        public UserService(IConfiguration config,AppDbContext db,ITokenService tokenService)
        {
            this.config = config;
            this.db = db;
            this.tokenService = tokenService;
        }

        public async Task<UserDto> AuthenticateUser(LoginDto model)
        {
            try
            {
                var userDto = new UserDto();
                var userInDb = await db.AppUsers.SingleOrDefaultAsync(x => x.Email == model.Email && x.Password == model.Password);
                if (userInDb == null)
                {
                    return null;
                }
                userDto.Id = userInDb.Id;
                userDto.Email = userInDb.Email;
                userDto.IsLocked = false;
                userDto.Roles = db.AppUserRoles.Where(x => x.AppUserId == userInDb.Id).Select(x => x.Role).ToList();
                userDto.Token = await tokenService.GenerateToken(userInDb);
                return userDto;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public Task<bool> CheckUserAvailability(string username)
        {
            return db.AppUsers.AnyAsync(x => x.UserName == username ||  x.Email == username);
        }

        public Task<bool> IsUserExists(string userId)
        {
            return db.AppUsers.AnyAsync(x => x.Id == userId);
        }

        public async Task<ResultDto> RegisterUser(UserCreateDto model)
        {
            try
            {
                var isUserExists = await CheckUserAvailability(model.Email);
                List<Error> errors = new List<Error>();
                var result = new ResultDto(false, errors);
                if (isUserExists)
                {
                    errors.Add(new Error("Duplicate Username", "Email/Username is already taken"));
                    return result;
                }
                var userToRegister = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Password = model.Password,
                    FullName = model.FullName
                };
                
                var role = new AppUserRole
                {
                    AppUserId = userToRegister.Id,
                    Role = UserRoles.Customer
                };
                db.AppUsers.Add(userToRegister);
                db.AppUserRoles.Add(role);
                await db.SaveChangesAsync();
                result.Succedded = true;
                return result;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
       
    }
}
