using Microsoft.EntityFrameworkCore;
using NewsApp.API.Data.Entities;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NewsApp.API.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }


        public DbSet<Comment> Comments { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
            
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new()
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);
            
            
        }
    }
}
