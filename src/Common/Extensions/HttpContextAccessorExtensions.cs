using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Common.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static string GetUserId
            (this ClaimsPrincipal user) => user
            .FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Ошибка авторизации.");
    }
}
