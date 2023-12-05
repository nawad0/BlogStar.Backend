using BlogStar.Backend.Data;
using BlogStar.Backend.Interfaces;
using BlogStar.Backend.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Services
{
    public class UserRepository : IUserRepository
    {
        private List<UserDto> _users => new()
    {
        new UserDto("John", "123"),
        new UserDto("Monica", "123"),
        new UserDto("string", "string")
    };

        public UserDto GetUser(UserModel userModel) =>
    _users.FirstOrDefault(u =>
        string.Equals(u.UserName, userModel.UserName) &&
        string.Equals(u.Password, userModel.Password)) ??
        throw new DllNotFoundException("User not found");

    }

}
