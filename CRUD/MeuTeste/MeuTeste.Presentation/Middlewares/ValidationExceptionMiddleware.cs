using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace MeuTeste.Presentation.Middlewares
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;

        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Erro de validação: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.ContentType = "application/json";

            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray());

            var response = new ProblemDetails
            {
                Title = "Erro de validação",
                Status = StatusCodes.Status400BadRequest,
                Detail = "Um ou mais campos de validação falharam",
                Instance = context.Request.Path
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;

            return context.Response.WriteAsJsonAsync(new
            {
                response.Title,
                response.Status,
                response.Detail,
                response.Instance,
                Errors = errors
            });
        }
    }
}
