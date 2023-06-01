// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerHost.Quickstart.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POSManager.Identity.Data;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System.Reflection;

namespace POSManager.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var seed = args.Contains("/seed");

        if (seed)
        {
            args = args.Except(new[] { "/seed" }).ToArray();
        }

        var builder = WebApplication.CreateBuilder(args);

        const string loggerTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u4}]<{ThreadId}> [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";

        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var logfile = Path.Combine(baseDir, "App_Data", "logs", "log.txt");
        var loggingLevelSwitch = new LoggingLevelSwitch();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        var appName = builder.Environment.ApplicationName;
        var env = builder.Environment.EnvironmentName;
        var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "-";

        try
        {
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
                            .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate));

            builder.Services.AddControllersWithViews();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<IdentityUser>()
                .AddDeveloperSigningCredential()
                .AddProfileService<IdentityProfileService>();

            //.AddTestUsers(TestUsers.Users)

            // this adds the config data from DB (clients, resources, CORS)
            //.AddConfigurationStore(options =>
            //{
            //    options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);
            //})
            // this adds the operational data from DB (codes, tokens, consents)
            //.AddOperationalStore(options =>
            //{
            //   options.ConfigureDbContext = builder => builder.UseSqlite(connectionString);

            // this enables automatic token cleanup. this is optional.
            //    options.EnableTokenCleanup = true;
            //})



            builder.Services.AddAuthentication();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            //if (seed)
            //{
            //    Log.Information("Seeding database...");
            //    //var config = app.Services.GetRequiredService<IConfiguration>();
            //    //var connectionString = config.GetConnectionString("DefaultConnection");
            //    SeedData.EnsureSeedData(connectionString);
            //    Log.Information("Done seeding database.");
            //    return;
            //}

            Log.Information("Starting host...");
            app.Run();
        }
        catch (Exception error)
        {
            if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Fatal(error, "POSManager.Identity crashed on start.");
        }
        finally
        {
            Log.Information("POSManager.Identity closing...");
            Log.Information("====================================================================\r\n");
            Log.CloseAndFlush();
        }
    }
}