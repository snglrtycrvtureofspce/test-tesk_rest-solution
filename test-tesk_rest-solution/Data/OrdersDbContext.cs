using Microsoft.EntityFrameworkCore;
using test_tesk_rest_solution.Data.Entities;

namespace test_tesk_rest_solution.Data;

public class OrdersDbContext : DbContext
{
    public virtual DbSet<OrderEntity> Orders { get; set; }
    
    public OrdersDbContext(DbContextOptions<OrdersDbContext> opt) : base(opt) { }

    public OrdersDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderEntity>(e =>
        {
            e.Property(p => p.Currency).HasConversion<string>();
            e.Property(p => p.Status).HasConversion<string>();
        });

        base.OnModelCreating(modelBuilder);
    }
}