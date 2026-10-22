using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using FourDigitCore.Application;
using FourDigitCore.Infrastructure;
using FourDigitCore.Infrastructure.Data;
using FourDigitCore.Infrastructure.Data.Seed;
using Serilog;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;

namespace FourDigitCore.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        // 0. Load environment variables from .env file
        DotNetEnv.Env.TraversePath().Load();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.File("logs/fourdigitcore-api-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            Log.Information("Starting up FourDigitCore (6174) API...");

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            // 1. Add Clean Architecture Layers
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // 2. Add Controllers
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // 3. Configure JWT Authentication
            var jwtSecret = Environment.GetEnvironmentVariable("JWT_KEY") 
                ?? builder.Configuration["Jwt:Secret"] 
                ?? "FourDigitCore6174SuperSecretKey2026MustBeAtLeast32BytesLong!";
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") 
                ?? builder.Configuration["Jwt:Issuer"] 
                ?? "FourDigitCoreApi";
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") 
                ?? builder.Configuration["Jwt:Audience"] 
                ?? "FourDigitCoreAdmin";

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // 4. Forwarded Headers for Cloud Reverse Proxies
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });

            // 5. Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
                options.AddPolicy("AllowFourDigitCoreClients", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // 6. Configure OpenAPI
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // 7. HTTP Pipeline
            app.UseForwardedHeaders();

            // Enable OpenAPI & Scalar API reference
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.Title = "FourDigitCore (6174) Systems API";
                options.Theme = ScalarTheme.DeepSpace;
                options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
                options.Authentication = new ScalarAuthenticationOptions
                {
                    PreferredSecuritySchemes = ["Bearer"]
                };
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("AllowFourDigitCoreClients");

            app.UseAuthentication();
            app.UseAuthorization();

            // Admin Invitation Acceptance UI route
            app.MapGet("/accept-invitation", async context =>
            {
                var webRoot = app.Environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var htmlPath = Path.Combine(webRoot, "accept-invitation.html");
                if (File.Exists(htmlPath))
                {
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.SendFileAsync(htmlPath);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
            });

            // Root & Health check endpoints
            app.MapMethods("/", ["GET", "HEAD"], () => Results.Ok(new 
            { 
                status = "healthy", 
                system = "FourDigitCore 6174 Sub-Millisecond Kernel API", 
                version = "1.0.0-net10.0",
                docs = "/scalar/v1" 
            }));
            
            app.MapMethods("/health", ["GET", "HEAD"], () => Results.Ok(new 
            { 
                status = "healthy", 
                timestamp = DateTime.UtcNow 
            }));

            app.MapControllers();

            // 8. Auto-Migrate and Seed Database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();
                    Log.Information("Applying EF Core migrations if any...");
                    await context.Database.MigrateAsync();

                    Log.Information("Seeding FourDigitCore initial data...");
                    await DatabaseSeeder.SeedAsync(context);
                    Log.Information("FourDigitCore database successfully initialized and seeded!");
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Database migration/seed skipped or encountered an error.");
                }
            }

            await app.RunAsync();
        }
        catch (Exception ex) when (ex is not HostAbortedException)
        {
            Log.Fatal(ex, "FourDigitCore API application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
