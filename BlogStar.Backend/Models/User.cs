using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public record UserDto(string UserName, string Password);

    public record UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Связь с избранными блогами
        public List<Blog> FavoriteBlogs { get; set; } = new List<Blog>();

        // Связь с избранными статьями
        public List<Article> FavoriteArticles { get; set; } = new List<Article>();
    }

}
