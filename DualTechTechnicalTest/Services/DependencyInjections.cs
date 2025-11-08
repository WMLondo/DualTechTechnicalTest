using DualTechTechnicalTest.Services.Contracts;

namespace DualTechTechnicalTest.Services;

public static class DependencyInjections
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}