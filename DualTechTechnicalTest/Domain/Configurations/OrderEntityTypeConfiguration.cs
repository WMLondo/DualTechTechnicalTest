using DualTechTechnicalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DualTechTechnicalTest.Domain.Configurations;

public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Client).WithMany(x => x.Orders).HasForeignKey(x => x.ClientId);
        builder.HasMany(x => x.OrderDetails).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);
    }
}