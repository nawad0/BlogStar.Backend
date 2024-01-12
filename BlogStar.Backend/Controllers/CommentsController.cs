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

namespace BlogStar.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly BlogStarDbContext _context;

        public CommentsController(BlogStarDbContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
          if (_context.Comments == null)
          {
              return NotFound();
          }
            return await _context.Comments.ToListAsync();
        }

        //GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }


        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        [HttpGet("article")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAtricleComments(int articleId)
        {
            try
            {

                var comments = await _context.Comments
                    .Where(b => b.ArticleId == articleId)
                    .ToListAsync();

                return comments;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment(int articleId, [FromBody] Comment comment)
        {

            var currentUserName = HttpContext.User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == currentUserName);


            if (_context.Articles == null)
            {
                return Problem("Entity set 'BlogStarDbContext.Articles'  is null.");
            }
            comment.ArticleId = articleId;
            comment.AuthorName = currentUserName;
            comment.AuthorImagePath = currentUser.UserImagePath;
            comment.AuthorUserId = currentUser.UserId;
            comment.PublicationDate = DateTime.Now.ToString("ddd, dd MMM yyyy");
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.CommentId }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            if (_context.Comments == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return (_context.Comments?.Any(e => e.CommentId == id)).GetValueOrDefault();
        }
    }
}
