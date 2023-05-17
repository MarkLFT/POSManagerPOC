using Finbuckle.MultiTenant;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMultiTenant<TenantInfo>()
                .WithHttpRemoteStore(builder.Configuration.GetValue<string>("TenantUrl"), httpBuilderClient =>
                {
                    httpBuilderClient.AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.RetryAsync(2));
                });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
