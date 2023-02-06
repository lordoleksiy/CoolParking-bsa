using System.Reflection;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Services;

namespace CoolParking.WebAPI.Infrastructure;

public static class ConfigServices
{
    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddMyDependencyGroup(
            this IServiceCollection services)
    {
        services.AddTransient<ITimerService, TimerService>();
        services.AddTransient<ILogService, LogService>(x => new LogService($@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log"));
        services.AddSingleton<IParkingService, ParkingService>();

        return services;
    }
}

