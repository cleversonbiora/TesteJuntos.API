using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TesteJuntos.Application.Middleware
{
    public class ResponseExceptionHandler : Controller
    {
        private readonly RequestDelegate _next;

        public ResponseExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                Response(httpContext, ex.Errors.Select(x => x.ErrorMessage).ToList());
            }
            catch (ArgumentException ex)
            {
                Response(httpContext, new List<string>() { ex.Message });
            }
            catch (Exception ex)
            {
                Response(httpContext, new List<string>() { ex.Message }, ex);
            }
        }

        public new void Response(HttpContext context, IReadOnlyCollection<string> errors = null, Exception ex = null)
        {
            var body = JsonConvert.SerializeObject(new
            {
                success = false,
                errors,
                ex = (ex == null ? null : new { ex.Message, ex.StackTrace,  ex.Source})
            });
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/json";
            context.Response.WriteAsync(body);
        }
    }
    
    public static class ResponseExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseResponseExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseExceptionHandler>();
        }
    }
}
