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
        UserDto GetUser(UserModel userModel);
    }

}
