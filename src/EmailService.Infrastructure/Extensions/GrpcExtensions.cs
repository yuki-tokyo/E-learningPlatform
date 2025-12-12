using EmailService.Domain.Exceptions.Account;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EmailService.Infrastructure.Extensions
{
    public static class GrpcExtensions
    {
        public static string GetUserId(this ServerCallContext context)
        {
            var token = context.RequestHeaders
                .FirstOrDefault(h => h.Key == "authorization")?.Value
                ?.Replace("Bearer ", "");

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

            return jwt.Claims.First(c =>
                c.Type.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase)).Value;
        }

        public static Metadata GetAuthMetadata(this IHttpContextAccessor contextAccessor)
        {
            var httpContext = contextAccessor.HttpContext
                ?? throw new MetadataException("Ошибка контекста.");

            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault()
                ?? throw new MetadataException("JWT отсутствует.");

            var token = authHeader.Replace("Bearer ", "");

            return new Metadata
            {
                { "authorization", $"Bearer {token}" }
            };
        }
    }
}
