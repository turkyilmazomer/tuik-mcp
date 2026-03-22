using Microsoft.Extensions.DependencyInjection;
using TuikMcp.Domain.Interfaces;
using TuikMcp.Infrastructure.Services;

namespace TuikMcp.Infrastructure.DependencyInjection;

/// <summary>
/// Infrastructure katmanı servis kayıtları
/// </summary>
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Domain servis implementasyonları (gömülü JSON verilerini kullanır)
        services.AddSingleton<IPopulationService, TuikPopulationService>();
        services.AddSingleton<IUnemploymentService, TuikUnemploymentService>();
        services.AddSingleton<IInflationService, TuikInflationService>();
        services.AddSingleton<IEducationService, TuikEducationService>();
        services.AddSingleton<IProvinceUnemploymentService, TuikProvinceUnemploymentService>();

        return services;
    }
}
