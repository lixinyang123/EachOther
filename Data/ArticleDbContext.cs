using Microsoft.EntityFrameworkCore;
using EachOther.Models;

namespace EachOther.Data
{
    public class ArticleDbContext : DbContext
    {
        public ArticleDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(i=>i.Article)
                .WithMany(i=>i.Comments)
                .HasForeignKey(i=>i.ArticleId);
        }

        public DbSet<Article> Articles {get; set;}

        public DbSet<Comment> Comments {get; set;}
    }
}