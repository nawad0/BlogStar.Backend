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
   
        //public async Task<BlogStarDbContext> UpdateAsync(Villa entity)
        //{
        //    entity.UpdatedDate = DateTime.Now;
        //    _db.At.Update(entity);
        //    await _db.SaveChangesAsync();
        //    return entity;
        //}

        public Task<BlogStarDbContext> UpdateAsync(Article entity)
        {
            throw new NotImplementedException();
        }
    }
}
