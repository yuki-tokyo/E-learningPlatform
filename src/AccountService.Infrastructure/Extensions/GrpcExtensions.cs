using AccountService.Domain.Exceptions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Infrastructure.Extensions
{
    public static class GrpcExtensions
    {
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
