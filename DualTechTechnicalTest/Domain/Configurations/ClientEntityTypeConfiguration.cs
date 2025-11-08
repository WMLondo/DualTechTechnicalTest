using DualTechTechnicalTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DualTechTechnicalTest.Domain.Configurations;

public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");
        builder.HasMany(x => x.Orders).WithOne(x => x.Client).HasForeignKey(x => x.ClientId);
    }
}