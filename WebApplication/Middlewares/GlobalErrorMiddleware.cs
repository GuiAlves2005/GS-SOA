using System.Net;
using System.Text.Json;

namespace HarmonyWork.Middlewares
{
    public class GlobalErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Tenta rodar a aplicação normalmente
                await _next(context);
            }
            catch (Exception ex)
            {
                // Se der erro, cai aqui
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.",
                Detailed = exception.Message // Em produção, esconderíamos isso
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}