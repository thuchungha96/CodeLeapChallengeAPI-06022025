using CodeLeapChallengeAPI_06022025.Data.Class;
using CodeLeapChallengeAPI_06022025.Data.Context;
using Newtonsoft.Json;
using System.Net;

namespace CodeLeapChallengeAPI_06022025
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "Unhandled exception occurred.");

            // Use IServiceScopeFactory to create a scope for DbContext
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var errorLoggingService = scope.ServiceProvider.GetRequiredService<IErrorLoggingService>();
                await errorLoggingService.LogErrorAsync(context, exception);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                status = context.Response.StatusCode,
                message = "An unexpected error occurred."
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
