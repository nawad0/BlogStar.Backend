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
            public DbSet<UserModel> Users { get; set; }
           
            public DbSet<Article> Articles { get; set; }
            public DbSet<Comment> Comments { get; set; }
            public DbSet<Like> Likes { get; set; }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Like>()
                    .HasOne(l => l.Article)
                    .WithMany(a => a.Likes)
                    .HasForeignKey(l => l.ArticleId);

                // Пример добавления одной статьи и нескольких лайков
                modelBuilder.Entity<Article>().HasData(
                    new Article
                    {
                        ArticleId = 1,
                        Title = "Пример статьи",
                        Content = "Содержание статьи...",
                        PublicationDate = DateTime.Now.ToString("yyyy-MM-dd"),
                    }
                );

                modelBuilder.Entity<Like>().HasData(
                    new Like { LikeId = 1, UserId = 1, ArticleId = 1 },
                    new Like { LikeId = 2, UserId = 2, ArticleId = 1 },
                    new Like { LikeId = 3, UserId = 3, ArticleId = 1 }
                );

                base.OnModelCreating(modelBuilder);
            }



    }
}
