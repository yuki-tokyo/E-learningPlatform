using Prometheus;
using SearchService.API.gRPC.Services;
using SearchService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpMetrics(options =>
{
    options.AddCustomLabel("host", context => context.Request.Host.Host);
});

app.MapMetrics();

app.MapGrpcService<SearchGrpcServer>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
