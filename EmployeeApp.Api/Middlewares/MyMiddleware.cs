namespace EmployeeApp.Api.Middlewares
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public MyMiddleware(RequestDelegate next,IConfiguration config)
        {
            _next=next;
            _config=config;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            var path=context.Request.Path;
            var company = _config["Company"];
            Console.WriteLine($"Path : {path}  and Company Name is {company}");
            await _next(context);
            Console.WriteLine($"REsponse :{context.Response.StatusCode}");
        }
    }
}
