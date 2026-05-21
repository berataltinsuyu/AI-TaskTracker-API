using System.Net;
using System.Text.Json;
using AITaskTracker.API.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AITaskTracker.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment env)
        {
          _next = next;
          _logger = logger;
          _env = env;
        }

    public async Task InvokeAsync (HttpContext context)
    {
      try
      {
          await _next(context);
      }
      catch (Exception ex)
      {
          _logger.LogError(ex,"An unexpected error occurred");

          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          context.Response.ContentType = "application/json";

          var message = _env.IsDevelopment() ? ex.Message : "An internal server error occurred.";
          var response = ApiResponse<object>.ErrorResponse(message);
          
          var json = JsonSerializer.Serialize(response);

          await context.Response.WriteAsync(json);
      }
    }
}
