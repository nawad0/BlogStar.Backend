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

        [HttpPost("article")]
        [Authorize]
        public ActionResult AddArticle([FromQuery] int articleId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized("User not authenticated.");
                }

                var currentUserName = HttpContext.User.Identity.Name;
                var currentUser = _dbContext.Users.FirstOrDefault(u => u.UserName == currentUserName);

                if (currentUser == null)
                {
                    return Unauthorized("User not found.");
                }

                // Инициализируем FavoriteArticles, если это еще не сделано
                currentUser.FavoriteArticles ??= "";

                // Проверяем, содержится ли articleId в списке FavoriteArticles
                if (currentUser.FavoriteArticles.Contains(articleId.ToString()))
                {
                    // Если содержится, удаляем
                    currentUser.FavoriteArticles = string.Join(",", currentUser.FavoriteArticles
                        .Split(',')
                        .Where(id => id != articleId.ToString()));
                }
                else
                {
                    // Если не содержится, добавляем
                    currentUser.FavoriteArticles = string.IsNullOrEmpty(currentUser.FavoriteArticles)
                        ? articleId.ToString()
                        : $"{currentUser.FavoriteArticles},{articleId}";
                }

                // Сохраняем изменения в базе данных
                _dbContext.SaveChanges();

                return Ok(articleId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
       
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            return await _dbContext.Users.ToListAsync();
        }
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<UserModel>> GetUserModel()
        {
            if (_dbContext.Users == null)
            {
                return NotFound();
            }
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);


            if (currentUser == null)
            {
                return NotFound();
            }

            return currentUser;
        }
        [HttpPut("user/{id}")]
        [Authorize]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return BadRequest();
            }

            // Fetch related comments, articles, and blogs
            var relatedComments = await _dbContext.Comments
                .Where(c => c.AuthorUserId == userModel.UserId)
                .ToListAsync();

            var relatedArticles = await _dbContext.Articles
                .Where(a => a.AuthorUserId == userModel.UserId)
                .ToListAsync();

            var relatedBlogs = await _dbContext.Blogs
                .Where(b => b.OwnerUserId == userModel.UserId)
                .ToListAsync();

            // Handle the file upload
            //if (file != null && file.Length > 0)
            //{
            //    var imagePath = await SaveImage(file);
            //    userModel.UserImagePath = imagePath; // Update the path in userModel
            //}

            // Update userModel in the context
            _dbContext.Entry(userModel).State = EntityState.Modified;

            // Update related comments, articles, and blogs
            foreach (var comment in relatedComments)
            {
                comment.AuthorName = userModel.UserName;
                comment.AuthorImagePath = userModel.UserImagePath;
            }

            foreach (var article in relatedArticles)
            {
                article.AuthorUserName = userModel.UserName;
                article.AuthorImagePath = userModel.UserImagePath;
            }

            foreach (var blog in relatedBlogs)
            {
                blog.UserName = userModel.UserName;
                blog.AuthorImagePath = userModel.UserImagePath;
            }

            try
            {
                // Save changes to the database
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(new { message = "User updated successfully" });
        }


        private async Task<string> SaveImage(IFormFile file)
        {
            var filePath = Path.Combine("D:\\projects\\BlogStar.Frontend\\BlogStar.Frontend\\BlogStar.Frontend\\src\\assets\\", file.FileName);
            var filePath1 = Path.Combine("src\\assets\\", file.FileName);
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the path where the image is saved
            return filePath1;
        }

        [HttpGet("favorite-articles")]
        [Authorize] // Require authentication to access this endpoint
        public ActionResult<List<Article>> GetFavoriteArticles()
        {
            try
            {
                // Ensure the user is authenticated
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized("User not authenticated.");
                }

                // Получаем имя текущего пользователя из контекста аутентификации
                var currentUserName = HttpContext.User.Identity.Name;

                // Получаем пользователя из базы данных по его имени
                var currentUser = _dbContext.Users.FirstOrDefault(u => u.UserName == currentUserName);

                if (currentUser == null)
                {
                    return Unauthorized("User not found.");
                }

                // Разбиваем строку на список (используя запятую как разделитель)
                var favoriteArticleIds = currentUser.FavoriteArticles?.Split(',')
                    .Where(id => !string.IsNullOrEmpty(id))
                    .Select(id => int.TryParse(id, out var parsedId) ? parsedId : -1)
                    .Where(parsedId => parsedId != -1)
                    .ToList();

                if (favoriteArticleIds == null)
                {
                    // Handle the case where FavoriteArticles is null
                    favoriteArticleIds = new List<int>();
                }

                // Получаем статьи из базы данных по ID
                var favoriteArticles = _dbContext.Articles.Where(a => favoriteArticleIds.Contains(a.ArticleId)).ToList();

                return Ok(favoriteArticles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
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
                expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    
}



