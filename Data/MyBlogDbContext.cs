using Microsoft.EntityFrameworkCore;
using MVCBlog.Models.Domain;

namespace MVCBlog.Data
{
    public class MyBlogDbContext : DbContext
    {
        public MyBlogDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
