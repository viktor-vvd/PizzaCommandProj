using Microsoft.EntityFrameworkCore;

namespace PizzaCommandProj_Management.Models
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
            //SampleData.Initialize(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Order>().HasOne(u => u.DishId).WithOne(p => p.Id);
        }
    }
}