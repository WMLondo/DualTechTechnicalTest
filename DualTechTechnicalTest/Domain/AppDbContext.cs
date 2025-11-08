using DualTechTechnicalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DualTechTechnicalTest.Domain;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}