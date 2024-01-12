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
using System.Security.Claims;
using Humanizer;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogStar.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly BlogStarDbContext _context;

        public BlogsController(BlogStarDbContext context)
        {
            _context = context;
        }

        // GET: api/Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
          if (_context.Blogs == null)
          {
              return NotFound();
          }
            return await _context.Blogs.ToListAsync();
        }

        // GET: api/Blogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
          if (_context.Blogs == null)
          {
              return NotFound();
          }
            var blog = await _context.Blogs.FindAsync(id);

            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }
        [HttpGet("user-blogs")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Blog>>> GetUserBlogs()
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
                var userBlogs = await _context.Blogs
                    .Where(b => b.OwnerUserId == currentUser.UserId)
                    .ToListAsync();

                return userBlogs;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT: api/Blogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBlog(int id, Blog blog)
        {
            if (id != blog.BlogId)
            {
                return BadRequest();
            }
            //Blog newblog = await _context.Blogs.FirstOrDefaultAsync(b => b.BlogId == id);
            //blog.UserName = newblog.UserName;
            //blog.BlogId = newblog.BlogId;
            //blog.CreationDate = newblog.CreationDate;
            //blog.OwnerUserId = newblog.OwnerUserId;

            _context.Entry(blog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
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


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Blog>> PostBlog(Blog blog)
        {
            try
            {
                var currentUserName = HttpContext.User.Identity.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);




                blog.OwnerUserId = currentUser.UserId;
                blog.CreationDate = DateTime.Now.ToString("ddd, dd MMM yyyy");
                blog.UserName = currentUserName;
                blog.AuthorImagePath = currentUser.UserImagePath;
                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync();

                // The blog object now has the automatically generated BlogId
                return CreatedAtAction("GetBlog", new { id = blog.BlogId }, blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }



        // DELETE: api/Blogs/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (_context.Blogs == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BlogExists(int id)
        {
            return (_context.Blogs?.Any(e => e.BlogId == id)).GetValueOrDefault();
        }
    }
}
