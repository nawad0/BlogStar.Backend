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

    //}CheckIfUsernameExists
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message)
            : base(message)
        {
        }
    }
    public class UserRepository : IUserRepository
    {
        private readonly BlogStarDbContext _dbContext;



        public bool CheckIfUsernameExists(string username, BlogStarDbContext dbContext)
        {
            // Check if a user with the specified username exists in the database
            var userExists = dbContext.Users.Any(u => u.UserName == username);

            return userExists;
        }

        public UserDto GetUser(LoginRequest userModel, BlogStarDbContext dbContext)
        {
            // Поиск пользователя с таким логином в базе данных
            var userEntity = dbContext.Users.FirstOrDefault(u => u.UserName == userModel.UserName);

            // Если пользователь с таким логином не найден
            if (userEntity == null)
            {
                return null;
                //throw new AuthenticationException("Неверный логин.");
            }

            // Проверка пароля
            if (userEntity.Password != userModel.Password)
            {
                return null;
                //throw new AuthenticationException("Неверный пароль.");
            }

            // Map the UserEntity to UserDto or create a UserDto instance based on your needs
            var userDto = new UserDto(userEntity.UserName, userEntity.Password);

            return userDto;
        }


    }

}
