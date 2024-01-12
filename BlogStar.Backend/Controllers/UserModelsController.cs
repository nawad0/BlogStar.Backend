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
    public class UserModelsController : ControllerBase
    {
        private readonly BlogStarDbContext _context;

        public UserModelsController(BlogStarDbContext context)
        {
            _context = context;
        }

        // GET: api/UserModels
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        //{
        //  if (_context.Users == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Users.ToListAsync();
        //}

        // GET: api/UserModels/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<UserModel>> GetUserModel(int id)
        //{
        //  if (_context.Users == null)
        //  {
        //      return NotFound();
        //  }
        //    var userModel = await _context.Users.FindAsync(id);

        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return userModel;
        //}
       

        // PUT: api/UserModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserModel(int id, UserModel userModel)
        {
            if (id != userModel.UserId)
            {
                return BadRequest();
            }

            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(id))
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

        // POST: api/UserModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'BlogStarDbContext.Users'  is null.");
          }
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.UserId }, userModel);
        }

        // DELETE: api/UserModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var userModel = await _context.Users.FindAsync(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _context.Users.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserModelExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
