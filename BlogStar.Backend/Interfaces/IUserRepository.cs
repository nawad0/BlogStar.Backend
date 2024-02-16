using BlogStar.Backend.Data;
using BlogStar.Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Interfaces
{
    public interface IUserRepository
    {
        UserDto GetUser(LoginRequest userModel, BlogStarDbContext _dbContext);
        public bool CheckIfUsernameExists(string username, BlogStarDbContext dbContext);
    }

}
