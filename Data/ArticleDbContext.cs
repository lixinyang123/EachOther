using Microsoft.EntityFrameworkCore;
using EachOther.Models;

namespace EachOther.Data
{
    public class ArticleDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(i=>i.Article)
                .WithMany(i=>i.Comments)
                .HasForeignKey(i=>i.ArticleId);

            modelBuilder.Entity<Reply>()
                .HasOne(i=>i.Comment)
                .WithMany(i=>i.Replies)
                .HasForeignKey(i=>i.CommitId);
        }

        public DbSet<Article> Articles {get; set;}

        public DbSet<Comment> Comments {get; set;}

        public DbSet<Reply> Replies {get; set;}
    }
}