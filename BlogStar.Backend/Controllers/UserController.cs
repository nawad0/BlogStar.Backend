using BlogStar.Backend.Data;
using BlogStar.Backend.Interfaces;
using BlogStar.Backend.Models;
using BlogStar.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly BlogStarDbContext _dbContext;
        public AuthController(IConfiguration configuration, IUserRepository userRepository, BlogStarDbContext dbContext)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        //[AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest loginModel)
        {

            // Проверка учетных данных пользователя
            var userDto = _userRepository.GetUser(loginModel, _dbContext);
           

            if (userDto == null)
            {
                return Unauthorized();
            }
       
            // Генерация JWT токена
            var token = GenerateJwtToken(userDto);

            return Ok(new { Token = token });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegistrationRequest registerModel)
        {
            // Validate registration model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            // Create a new User entity
            var newUser = new UserModel
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                RegistrationDate = DateTime.UtcNow,
                Password = registerModel.Password,
                // Add other user properties as needed
            };

            // Hash the password (you should use a secure password hashing algorithm)


            // Save the user to the database using your repository
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            // Generate JWT token

            return Ok();
        }

        private string GenerateJwtToken(UserDto userDto)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userDto.UserName),
            
            // Добавьте другие необходимые утверждения (claims) по вашему усмотрению
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}



