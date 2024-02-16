using BlogStar.Backend.Data;
using BlogStar.Backend.Models;
using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace BlogStar.Backend.Repository.IRepository
{
    public interface IArticleRepository : IRepository<Article>
    {

        Task<BlogStarDbContext> UpdateAsync(Article entity);



    }
}
