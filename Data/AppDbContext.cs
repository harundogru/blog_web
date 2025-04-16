using Microsoft.EntityFrameworkCore; // Bu gerekli!
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using Blog_Web.Models; 

namespace Blog_Web.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Blog> Blogs { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<Category> Categories { get; set; }
}