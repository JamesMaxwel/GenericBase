using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GenericBase.Application.Helpers
{
    public static class HttpContextHelpers
    {
        public static string? GetUserId(this HttpContext httpContext)
            => httpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        public static string? GetUserIpAddress(this HttpContext httpContext)
            => httpContext.Connection.RemoteIpAddress?.ToString();

        public static string GetUserAgent(this HttpContext httpContext)
            => httpContext.Request.Headers["User-Agent"].ToString();

        public static string? GetAccessToken(this HttpContext httpContext)
        {
            var authorizationHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                return authorizationHeader["Bearer ".Length..];
            }
            return null;
        }

        public static bool IsAuthenticated(this HttpContext httpContext)
            => httpContext.User?.Identity?.IsAuthenticated ?? false;

        public static string GetCurrentUrl(this HttpContext httpContext)
        => $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";

    }
}