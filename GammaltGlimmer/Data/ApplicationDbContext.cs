using GammaltGlimmer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GammaltGlimmer.Data
{
    // IdentityDbContext contains all the user tables
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed categories
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 1, CategoryName = "Transport" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 2, CategoryName = "Hushåll" });
            modelBuilder.Entity<Category>().HasData(new Category { CategoryId = 3, CategoryName = "Dekoration" });

            //seed product

            modelBuilder.Entity<Item>().HasData(new Item
            {
                ItemId = "TTT123456",
                Name = "Trähäst dekoration",
                Description = "Trähäst InMemory!",
                ReleaseYear = 1990,
                StartPrice = 15,
                PurchaseCost = 10,
                Status = "Kvar i lager",
                FinalPrice = 0,
                CategoryId = 3,
                CreatedBy = "System",
            });
        }
    }
}