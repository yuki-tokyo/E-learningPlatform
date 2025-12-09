using AuthService.API.gRPC.Services;
using AuthService.Application.Services;
using AuthService.Domain.Interfaces;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Repos;
using AuthService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AppAuthService = AuthService.Application.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DB
builder.Services.AddInfrastructure(builder.Configuration);

// Jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };

    });

// gRPC Server
builder.Services.AddGrpc();

// gRPC Client
builder.Services.AddGrpcClient<AuthApi.AuthApiClient>(o =>
{
    o.Address = new Uri("https://localhost:3001");
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    // Ignore SSL 
    handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});


// DI-containers
builder.Services.AddScoped<IAuthGrpcClient, AuthGrpcClient>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AppAuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGrpcService<AuthGrpcServer>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
