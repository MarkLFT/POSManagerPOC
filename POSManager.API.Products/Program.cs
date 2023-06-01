using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Authorization;
using POSManager.API.Products.Scope;
using Serilog.Core;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;
using POSManager.API.Products.Data;
using Microsoft.EntityFrameworkCore;
using POSManager.API.Products.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

namespace POSManager.API.Products;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        const string loggerTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var logfile = Path.Combine(baseDir, "App_Data", "logs", "log.txt");
        var loggingLevelSwitch = new LoggingLevelSwitch();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();


        try
        {
            var appName = builder.Environment.ApplicationName;
            var env = builder.Environment.EnvironmentName;
            var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "-";

            Log.Information("====================================================================");
            Log.Information($"Application Starts. Version: {version}");
            Log.Information($"Application Directory: {AppDomain.CurrentDomain.BaseDirectory}");
            Log.Information($"Application Environment: {env}");
            Log.Information("====================================================================");
            Log.Information($"{appName} Service starting...");

            builder.Host.UseSerilog((ctx, lc) => lc
            .ReadFrom.Configuration(ctx.Configuration)
            .Enrich.WithProperty("ApplicationName", appName)
            .Enrich.WithProperty("Environment", env)
            .Enrich.WithProperty("ApplicationVersion", version)
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
            .WriteTo.File(logfile, LogEventLevel.Information, loggerTemplate, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7));

            var tenantConnectionString = builder.Configuration.GetConnectionString("Tenants");

            builder.Services.AddDbContext<MultiTenantStoreDbContext>(opts =>
            {
                opts.UseSqlServer(tenantConnectionString);
            });

            // Add services to the container.
            builder.Services.AddMultiTenant<CustomTenant>()
                            .WithEFCoreStore<MultiTenantStoreDbContext, CustomTenant>()
                            .WithClaimStrategy("tenant")
                            .WithStaticStrategy("demo");


            var domain = builder.Configuration["Authentication:Schemes:Bearer:Authority"];

            builder.Services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidTypes = new[] { "at+jwt" },
                    ValidateAudience = false
                };

                options.MapInboundClaims = true;
            });

            if (domain is not null)
            {
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy("products.read", policy => policy.RequireClaim("scope", "products.read", "products.write"));

                    options.AddPolicy("products.write", policy => policy.RequireClaim("scope", "products.write"));
                });
            }

            builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            builder.Services.AddDbContext<IAppDbContext, AppDbContext>();

            builder.Services.AddScoped<IRepository, Repository>();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseMultiTenant();

            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();

                app.UseSwagger();
                app.UseSwaggerUI();

                app.UseDeveloperExceptionPage();

                IdentityModelEventSource.ShowPII = true;
            }

            app.UseAuthorization();

            app.MapControllers();

            await SetupStore(app.Services, app.Configuration);

            app.Run();
        }

        catch (Exception error)
        {
            if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Fatal(error, "Products API crashed on start.");
        }
        finally
        {
            Log.Information("Products API closing...");
            Log.Information("====================================================================\r\n");
            Log.CloseAndFlush();
        }

        static async Task SetupStore(IServiceProvider sp, IConfiguration configuration)
        {
            var scopeServices = sp.CreateScope().ServiceProvider;
            var store = scopeServices.GetRequiredService<IMultiTenantStore<CustomTenant>>();

            var tenants = await store.GetAllAsync();

            if (!tenants.Any())
            {
                store.TryAddAsync(new CustomTenant { Id = "joescafe", Identifier = "joescafe", Name = "Joe's Cafe", ConnectionString = configuration.GetConnectionString("Default"), ConnectionStringProducts = configuration.GetConnectionString("Products"), ConnectionStringInvoices = configuration.GetConnectionString("Invoices") }).Wait();
                store.TryAddAsync(new CustomTenant { Id = "emdesign", Identifier = "emdesign", Name = "Em Design", ConnectionString = configuration.GetConnectionString("Default"), ConnectionStringProducts = configuration.GetConnectionString("Products"), ConnectionStringInvoices = configuration.GetConnectionString("Invoices") }).Wait();
                store.TryAddAsync(new CustomTenant { Id = "demo", Identifier = "demo", Name = "Demo Cafe", ConnectionString = configuration.GetConnectionString("Default"), ConnectionStringProducts = configuration.GetConnectionString("Products"), ConnectionStringInvoices = configuration.GetConnectionString("Invoices") }).Wait();
            }

            foreach (var tenant in await store.GetAllAsync())
            {
                await using var db = new AppDbContext(tenant);
                await db.Database.MigrateAsync();
            }
        }
    }
}