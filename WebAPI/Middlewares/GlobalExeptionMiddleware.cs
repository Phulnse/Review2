using Application.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace WebAPI.Middlewares
{
    public class GlobalExeptionMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExeptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExeptionAsync(context, ex);
            }
        }

        private async Task HandleExeptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            string jsonString = JsonConvert.SerializeObject(new Response(500, ex.Message));
            await context.Response.WriteAsync(jsonString, Encoding.UTF8);
        }
    }
}
