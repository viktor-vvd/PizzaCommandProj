using Microsoft.EntityFrameworkCore;
using PizzaCommandProj.Models;

namespace PizzaCommandProj.Models
{
    public class PizzaContext : DbContext
    {
        //DB arrays
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }

        public PizzaContext(DbContextOptions<PizzaContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            SampleData.Initialize(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>().HasOne(u => u.DishId).WithOne(p => p.Id);
        }

        public DbSet<PizzaCommandProj.Models.CartItem> CartItem { get; set; }
    }
}