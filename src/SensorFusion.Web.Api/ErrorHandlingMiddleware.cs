using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SensorFusion.Shared.Exceptions;

namespace SensorFusion.Web.Api
{
    public class ErrorHandlingMiddleware
    {
      private readonly RequestDelegate _next;
      private readonly ILogger _logger;

      public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
      {
        _next = next;
        _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
      }

      public async Task Invoke(HttpContext context)
      {
        try
        {
          await _next.Invoke(context);
        }
        catch (Exception ex)
        {
          await HandleExceptionAsync(context, ex);
        }
      }

      private async Task HandleExceptionAsync(HttpContext context, Exception exception)
      {
        switch (exception)
        {
          case NotAllowedException ex:
            await WriteErrorAsync(context, ex.Message,
              HttpStatusCode.Forbidden);
            _logger.LogError(exception, ex.Message);
            break;
          case BusinessLogicException ex:
            await WriteErrorAsync(context, ex.Message,
              HttpStatusCode.BadRequest);
            _logger.LogError(exception, ex.Message);
            break;
          default:
            await WriteErrorAsync(context, @"Something went wrong ¯\_(ツ)_/¯, try again later",
              HttpStatusCode.InternalServerError);
            _logger.LogError(exception, "Internal server error");
            break;
        }

      }

      private static async Task WriteErrorAsync(HttpContext context, string error, HttpStatusCode statusCode)
      {
        var result = JsonConvert.SerializeObject(new { error });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(result);
      }
    }
}