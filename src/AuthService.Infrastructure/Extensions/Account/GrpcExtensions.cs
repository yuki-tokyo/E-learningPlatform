using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthService.Infrastructure.Extensions.Account
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
    }
}
