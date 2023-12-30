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

namespace BlogStar.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly BlogStarDbContext _context;

        public ArticlesController(BlogStarDbContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
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

            return article;
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

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            if (_context.Articles == null)
            {
                return NotFound();
            }
            return await _context.Articles.ToListAsync();
        }
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
