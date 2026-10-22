using Microsoft.Extensions.DependencyInjection;
using FourDigitCore.Application.Services;

namespace FourDigitCore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFourDigitCorePublicService, FourDigitCorePublicService>();
        services.AddScoped<IFourDigitCoreAdminService, FourDigitCoreAdminService>();
        services.AddScoped<FourDigitCore.Application.Common.Interfaces.IAdminUserManagementService, AdminUserManagementService>();
        return services;
    }
}
