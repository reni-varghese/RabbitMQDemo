using EmployeeApp.Api.Data;
using EmployeeApp.Api.Mappings;
using EmployeeApp.Api.Messaging;
using EmployeeApp.Api.Middlewares;
using EmployeeApp.Api.Repositories;
using EmployeeApp.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
       options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbCon"));
});
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ClockSkew=TimeSpan.Zero
            


        };

    });
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddCors(cfg =>
{
    cfg.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:7280","http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
   
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddSingleton<RabbitMqPublisher>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();

});

var app = builder.Build();
app.UseExceptionHandler();
using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await RoleSeeder.SeedRoleAsync(roleManager);

}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseMiddleware<MyMiddleware>();
//app.Use(async (context, next) =>
//{
//    Console.WriteLine("Hello world before processing request");
//    var comp=Environment.GetEnvironmentVariable("Company");
//    Console.WriteLine(comp + "=================");
//    await next(context);
//    Console.WriteLine("Hello World after processing the request");

//});

app.Run();
