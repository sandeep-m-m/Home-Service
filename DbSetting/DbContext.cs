using Microsoft.EntityFrameworkCore;
using HomeService.Models;

namespace HomeService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<SampleData> Data { get; set; }
        public DbSet<UserInteraction> UserInteractions { get; set; }
        public DbSet<UserPurchase> UserPurchases { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleData>()
                .Property(x => x.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<SampleData>()
                .Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
