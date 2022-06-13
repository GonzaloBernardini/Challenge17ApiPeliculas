using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;


namespace Challenge17ApiPeliculas.Middlewares
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggerMiddleware> _logger;
        public LoggerMiddleware(RequestDelegate next,ILoggerFactory loger)
        {
            _next = next;
            _logger = loger.CreateLogger<LoggerMiddleware>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Algo esta saliendo mal{ex.Message}");

            }
            finally
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    _logger.LogInformation("El usuario {User}  hizo una peticion.", context.User.Identity.Name);
                }

            }
        }
    }
}
