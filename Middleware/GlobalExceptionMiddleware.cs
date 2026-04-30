using System.Net;
using System.Text.Json;
using AITaskTracker.API.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AITaskTracker.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
        {
          _next = next;
          _logger = logger;
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

          var response = ApiResponse<object>.ErrorResponse("An unexpected error occurred.");
          
          var json = JsonSerializer.Serialize(response);

          await context.Response.WriteAsync(json);
      }
    }
}