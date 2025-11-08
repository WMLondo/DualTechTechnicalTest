using DualTechTechnicalTest.Domain.Contracts;
using DualTechTechnicalTest.Domain.Repositories.Contracts;
using DualTechTechnicalTest.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DualTechTechnicalTest.Domain;

public static class DependencyInjections
{
    public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<AppDbContext>(options => 
                options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        return services;
    }
}