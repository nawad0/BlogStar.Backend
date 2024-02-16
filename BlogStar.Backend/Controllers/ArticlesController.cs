using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogStar.Backend.Data;
using BlogStar.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using MagicVilla_VillaAPI.Models;
using System.Net;
using BlogStar.Backend.Repository.IRepository;
using System.Text.Json.Serialization;

namespace BlogStar.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly BlogStarDbContext _context;
        protected APIResponse _response;
        private readonly IArticleRepository _db;

        public ArticlesController(BlogStarDbContext context, IArticleRepository db)
        {
            _context = context;
            _db = db;
            _response = new APIResponse();
        }

        [HttpGet]
        //[Authorize]
        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetArticles([FromQuery] string? search, int pageSize = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Article> articleList;

                articleList = await _db.GetAllAsync(pageSize: pageSize, pageNumber: pageNumber, includeProperties: "Likes");

                if (!string.IsNullOrEmpty(search))
                {
                    articleList = articleList.Where(u => u.Content.ToLower().Contains(search));
                }

                // Вычисляем количество лайков для каждой статьи
                foreach (var article in articleList)
                {
                    article.LikesCount = article.Likes.Count;
                }

                Pagination pagination = new Pagination()
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                };
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true
                };
                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = articleList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        //[ResponseCache(CacheProfileName = "Default30")]
        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Id cannot be equal to 0");
                    return BadRequest(_response);
                }

                // Используем Include для загрузки лайков вместе со статьей
                var villa = await _db.GetAsync(u => u.ArticleId == id, includeProperties: "Likes");

                if (villa == null)
                {
                    _response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Get Villa Error with Id" + id);
                    return NotFound(_response);
                }

                // Теперь у вас должно быть корректное количество лайков
                _response.Result = villa.Likes.Count;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Blog updated successfully" });
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpGet("blog-articles")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Article>>> GetUserBlogs(int blogId)
        {
            try
            {
                // Получаем имя текущего пользователя
                var currentUserName = HttpContext.User.Identity.Name;

                // Ищем пользователя в базе данных по имени
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);

                // Проверяем, найден ли пользователь
                if (currentUser == null)
                {
                    return Unauthorized(); // Пользователь не найден
                }

                // Получаем все блоги, созданные текущим пользователем
                var userBlogs = await _context.Articles
                    .Where(b => b.BlogId == blogId)
                    .ToListAsync();

                return userBlogs;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Article>> PostArticle(int blogId, [FromBody] Article article)
        {
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);

            if (_context.Articles == null)
            {
                return Problem("Entity set 'BlogStarDbContext.Articles' is null.");
            }

            // Don't set the ArticleId, let the database generate it
            article.BlogId = blogId;
            article.AuthorUserName = currentUserName;
            article.AuthorUserId = currentUser.UserId;
            article.AuthorImagePath= currentUser.UserImagePath;
            article.LikesJson = "[]";
            article.PublicationDate = DateTime.Now.ToString("ddd, dd MMM yyyy");

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            // Return the article with the generated ArticleId
            return CreatedAtAction("GetArticle", new { id = article.ArticleId }, article);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddLike([FromQuery] int articleId)
        {
            if (articleId <= 0)
            {
                return BadRequest("Invalid ArticleId.");
            }

            // Get the current user
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);

            if (currentUser == null)
            {
                return BadRequest("User not found.");
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.ArticleId == articleId);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            // Deserialize the JSON string to a List<Like>
            var likes = string.IsNullOrEmpty(article.LikesJson)
                ? new List<Like>()
                : JsonSerializer.Deserialize<List<Like>>(article.LikesJson);

            // Check if the user has already liked the article
            var existingLike = likes.FirstOrDefault(l => l.UserId == currentUser.UserId);

            if (existingLike != null)
            {
                // User has already liked the article, remove the like
                likes.Remove(existingLike);
            }
            else
            {
                // User hasn't liked the article, add a new like
                var newLike = new Like { UserId = currentUser.UserId };
                likes.Add(newLike);
            }

            // Serialize the List<Like> back to JSON string
            article.LikesJson = JsonSerializer.Serialize(likes);

            await _context.SaveChangesAsync();

            return Ok("Like updated successfully.");
        }

        [HttpGet("checklike")]
        [Authorize]
        public async Task<IActionResult> CheckLike([FromQuery] int articleId)
        {
            if (articleId <= 0)
            {
                return BadRequest("Invalid ArticleId.");
            }

            // Get the current user
            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);

            if (currentUser == null)
            {
                return BadRequest("User not found.");
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(a => a.ArticleId == articleId);

            if (article == null)
            {
                return NotFound("Article not found.");
            }

            // Deserialize the JSON string to a List<Like>
            var likes = string.IsNullOrEmpty(article.LikesJson)
                ? new List<Like>()
                : JsonSerializer.Deserialize<List<Like>>(article.LikesJson);

            // Check if the user has already liked the article
            var hasLiked = likes.Any(l => l.UserId == currentUser.UserId);

            return Ok(new { HasLiked = hasLiked });
        }



        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return (_context.Articles?.Any(e => e.ArticleId == id)).GetValueOrDefault();
        }
    }
}
