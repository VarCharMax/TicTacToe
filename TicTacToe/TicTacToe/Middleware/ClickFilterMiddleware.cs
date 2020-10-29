using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TicTacToe.Middleware
{
    public class ClickFilterMiddleware
    {
        private readonly RequestDelegate _next;

        public ClickFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("x-frame-options", "DENY");

            await _next.Invoke(context);
        }
    }
}
