using System;
using System.Net;
using System.Threading.Tasks;
using Abstract.Helpful.Lib;
using Abstract.Helpful.Lib.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Abstract.Helpful.AspNetCore
{
    public abstract class ExceptionToObjectMiddleware<TVisibleApiException, TError> where TVisibleApiException : Exception
    {
        private readonly RequestDelegate _next;

        protected ExceptionToObjectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TVisibleApiException visibleApiException)
            {
                HandleException(visibleApiException, out var error, out var statusCode);
                var serializedError = JsonConvert.SerializeObject(error);
                context.Response.StatusCode = statusCode.ToInt();
                await context.Response.WriteAsync(serializedError);
            }
            catch (Exception e)
            {
                StaticLogger.Log(e.ToPrettyDevelopersString());
                throw;
            }
        }

        protected abstract void HandleException(TVisibleApiException exception,
            out TError error,
            out HttpStatusCode statusCode);
    }
}