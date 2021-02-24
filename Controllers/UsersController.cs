using Bogus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
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

namespace RenCart.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var errors = new List<Error>();
            var result = new ResultDto(false,errors);
            var userDto = await userService.AuthenticateUser(model);
            if (userDto == null)
            {
                return BadRequest(result);
            }
            return Ok(userDto);
            
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto model)
        {
            try
            {
                var result = await userService.RegisterUser(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
        [HttpGet("seedusers")]
        public async Task<IActionResult> SeedUsers()
        {
            var faker = new Faker<UserCreateDto>()
                .RuleFor(x => x.FullName, y => y.Person.FullName)
                .RuleFor(x => x.Email, y => y.Person.Email)
                .RuleFor(x => x.Password, y => y.Internet.Password(4,true));
            var users = faker.Generate(10);
            foreach (var u in users)
            {
                await userService.RegisterUser(u);
            }
            return Ok(users);
        }
    }
}
