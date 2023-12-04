using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogStar.Backend.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Список избранных блогов
        public List<int> FavoriteBlogs { get; set; }
        // Список избранных статей
        public List<int> FavoriteArticles { get; set; }

    }

}
