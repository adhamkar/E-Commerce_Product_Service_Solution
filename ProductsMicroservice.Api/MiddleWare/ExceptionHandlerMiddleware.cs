namespace eCommerce.ProductsMicroservice.Api.MiddleWare
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.GetType().ToString()} :" +
                    $" {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"{ex.InnerException.GetType().ToString()} :" +
                        $" {ex.InnerException.Message}");
                }
                else
                {
                    _logger.LogError("{ExceptionType} {ExceptionMessage}",ex.GetType().ToString(),ex.Message);
                }
                    httpContext.Response.StatusCode = 500; //internal server error
                await httpContext.Response.WriteAsJsonAsync(new { Message = ex.Message, Type = ex.GetType().ToString() });
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
