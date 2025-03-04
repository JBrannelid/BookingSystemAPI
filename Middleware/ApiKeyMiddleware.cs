namespace BookingSystemAPI.Middleware
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;  // Access the ApiKey configuration
        }

        // part of the middleware convention in ASP.NET Core, Method Name has to be Invoke or InvokeAsyn
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Request Recieved.");

            string? configuredApiKey = _configuration["ApiKey"];
            var apiKey = context.Request.Headers["X-API-Key"].FirstOrDefault();

            if (apiKey != configuredApiKey)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access Denied! :( ");
                return;
            }

            await _next(context);
        }
    }
}