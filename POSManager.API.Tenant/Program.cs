using Finbuckle.MultiTenant;
using Serilog.Core;
using Serilog;
using System.Reflection;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

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
    var name = builder.Environment.ApplicationName.Split(".").Last();
    var env = builder.Environment.EnvironmentName;
    var version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "-";

    Log.Information("====================================================================");
    Log.Information($"Application Starts. Version: {version}");
    Log.Information($"Application Directory: {AppDomain.CurrentDomain.BaseDirectory}");
    Log.Information($"Application Environment: {env}");
    Log.Information("====================================================================");
    Log.Information("Tenants API Service starting...");

    builder.Host.UseSerilog((ctx, lc) => lc
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.WithProperty("ApplicationName", name)
    .Enrich.WithProperty("Environment", env)
    .Enrich.WithProperty("ApplicationVersion", version)
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(LogEventLevel.Information, loggerTemplate, theme: AnsiConsoleTheme.Literate)
    .WriteTo.File(logfile, LogEventLevel.Information, loggerTemplate, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7));


    // Add services to the container.

    builder.Services.AddMultiTenant<TenantInfo>()
                .WithConfigurationStore();

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

catch (Exception error)
{
    if (Log.Logger == null || Log.Logger.GetType().Name == "SilentLogger")
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
        .WriteTo.Console()
        .CreateLogger();

    Log.Fatal(error, "Tenants API crashed on start.");
}
finally
{
    Log.Information("Tenants API closing...");
    Log.Information("====================================================================\r\n");
    Log.CloseAndFlush();
}
