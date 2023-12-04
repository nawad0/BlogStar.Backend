using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogStar.Backend.Data
{
    public class BlogStarDbContext : DbContext
    {
        
            public BlogStarDbContext(DbContextOptions options) : base(options)
            {

            }
            //public DbSet<Contact> Contacts { get; set; }
        
    }
}
