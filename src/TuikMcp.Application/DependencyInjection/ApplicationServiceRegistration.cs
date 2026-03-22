using Microsoft.Extensions.DependencyInjection;
using TuikMcp.Application.Services;

namespace TuikMcp.Application.DependencyInjection;

/// <summary>
/// Application katmanı servis kayıtları
/// </summary>
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<PopulationAppService>();
        services.AddScoped<UnemploymentAppService>();
        services.AddScoped<InflationAppService>();
        services.AddScoped<EducationAppService>();
        services.AddScoped<ProvinceUnemploymentAppService>();

        return services;
    }
}
