namespace CatalogAPI.Middlewares 
{ 
    internal class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                var statusCode = ex switch
                {
                    FluentValidation.ValidationException => StatusCodes.Status400BadRequest,
                    CatalogAPI.Exceptions.ProductNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                };

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = ex.Message,
                    Detail = _env.IsDevelopment() ? ex.StackTrace : null
                };

                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = statusCode;
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }

}
