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
    //public class UserRepository : IUserRepository
    //{

    //    private List<UserDto> _users => new()
    //{
    //    new UserDto("John", "123"),
    //    new UserDto("Monica", "123"),
    //    new UserDto("string", "string")
    //};

    //    public UserDto GetUser(LoginRequest userModel) =>
    //_users.FirstOrDefault(u =>
    //    string.Equals(u.UserName, userModel.UserName) &&
    //    string.Equals(u.Password, userModel.Password)) ??
    //    throw new DllNotFoundException("User not found");

    //}
    public class UserRepository : IUserRepository
    {
        private readonly BlogStarDbContext _dbContext;

        

        public UserDto GetUser(LoginRequest userModel, BlogStarDbContext _dbContext)
        {
            // Fetch the user from the database
            var userEntity = _dbContext.Users
                .FirstOrDefault(u => u.UserName == userModel.UserName && u.Password == userModel.Password);

            // If user not found, throw an exception
            if (userEntity == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            // Map the UserEntity to UserDto or create a UserDto instance based on your needs
            var userDto = new UserDto(userEntity.UserName, userEntity.Password);
                
            

            return userDto;
        }
    }

}
