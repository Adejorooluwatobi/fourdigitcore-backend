using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FourDigitCore.Application.Common.Interfaces;
using FourDigitCore.Infrastructure.Data;
using FourDigitCore.Infrastructure.Services;

namespace FourDigitCore.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
            ?? configuration.GetConnectionString("DefaultConnection") 
            ?? configuration["DB_CONNECTION_STRING"];

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? configuration["DB_HOST"] ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? configuration["DB_PORT"] ?? "5432";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? configuration["DB_USER"] ?? "postgres";
            var pass = Environment.GetEnvironmentVariable("DB_PASS") ?? configuration["DB_PASS"] ?? "postgres";
            var name = Environment.GetEnvironmentVariable("DB_NAME") ?? configuration["DB_NAME"] ?? "fourdigitcore_db";
            connectionString = $"Host={host};Port={port};Database={name};Username={user};Password={pass};";
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IFileStorageService, CloudinaryFileStorageService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
