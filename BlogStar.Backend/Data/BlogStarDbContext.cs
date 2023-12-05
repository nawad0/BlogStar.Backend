using BlogStar.Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogStar.Backend.Data
{
    public class BlogStarDbContext : DbContext
    {
        
            public BlogStarDbContext(DbContextOptions options) : base(options)
            {

            }
            public DbSet<Blog> Blogs { get; set; }
            public DbSet<UserDto> Users { get; set; }
           
            public DbSet<Article> Articles { get; set; }
            public DbSet<Comment> Comments { get; set; }
        //public DbSet<Contact> Contacts { get; set; }

    }
}
