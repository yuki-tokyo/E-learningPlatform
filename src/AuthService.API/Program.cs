using AuthService.API.gRPC.Services;
using AuthService.Application.Services;
using AuthService.Application.Services.Email;
using AuthService.Domain.Interfaces.Email;
using AuthService.Domain.Interfaces.gRPC;
using AuthService.Infrastructure.Data;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Repos;
using AuthService.Protos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AppAuthService = AuthService.Application.Services.AuthService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Infrastructure
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGrpcService<AuthGrpcServer>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
