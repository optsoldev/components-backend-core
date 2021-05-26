using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Optsol.Components.Service.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Optsol.Components.Service.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> logger;
        
        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unexpected erro: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var erros = new List<string>();
            erros.Add("An error occurred whilst processing your request");
            erros.Add(exception.Message);

            var response = new Response(false, erros);

            return  context.Response.WriteAsJsonAsync(response);
        }

    }
}
