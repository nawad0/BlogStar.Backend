//using AutoMapper;
using BlogStar.Backend.Controllers;
using BlogStar.Backend.Data;
using BlogStar.Backend.Models;
using BlogStar.Backend.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BlogStar.Backend.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly BlogStarDbContext _db;
        
        public ArticleRepository(BlogStarDbContext db) :base(db) 
        {
            _db = db;
        }

        public async Task<Article> UpdateAsync(Article entity)
        {
            
            _db.Articles.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }


    }
}
