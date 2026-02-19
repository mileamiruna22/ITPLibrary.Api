using Dapper;
using ITPLibrary.Api.Core.Dtos;
using ITPLibrary.Api.Core.Services;
using ITPLibrary.Api.Data.Entities;
using ITPLibrary.Api.Data.Repositories;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MimeKit;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173",
                                             "http://127.0.0.1:5173",
                                             "https://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials(); 
                      });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ITPLibrary.Api", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("authToken"))
            {
                context.Token = context.Request.Cookies["authToken"];
            }
           
            else if (context.Request.Headers.ContainsKey("Authorization"))
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["authToken"];
    if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token);
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/minimal/books", async (IBookService bookService) =>
{
    var books = await bookService.GetPopularBooksAsync();
    return Results.Ok(books);
});

app.MapPost("/api/minimal/books", async (IBookService bookService, BookDto bookDto) =>
{
    await bookService.AddBookAsync(bookDto);
    return Results.Created($"/api/minimal/books/{bookDto.Id}", bookDto);
});

app.MapPost("/api/register", async (IUserService userService, RegisterUserDto userDto) =>
{
    var result = await userService.RegisterUserAsync(userDto);
    if (result == false)
    {
        return Results.Conflict("User with this email already exists.");
    }
    return Results.Created("/api/register", userDto);
});

app.MapPost("/api/login", async (HttpContext httpContext, IUserService userService, LoginUserDto userDto) =>
{
    var token = await userService.LoginUserAsync(userDto);
    if (token == null)
    {
        return Results.Unauthorized();
    }

    var cookieOptions = new CookieOptions
    {
        HttpOnly = true,      
        Secure = true,        
        SameSite = SameSiteMode.None, 
        Expires = DateTimeOffset.UtcNow.AddHours(24), 
        Path = "/"            
    };

    httpContext.Response.Cookies.Append("authToken", token, cookieOptions);

    return Results.Ok(new { message = "Login successful" });
});

app.MapGet("/api/me", (HttpContext httpContext) =>
{
    if (httpContext.User.Identity?.IsAuthenticated != true)
    {
        return Results.Unauthorized();
    }

    var identityName = httpContext.User.Identity.Name;

    return Results.Ok(new { message = "Authenticated", user = identityName });
})
.RequireAuthorization();

app.MapPost("/api/logout", (HttpContext httpContext) =>
{
    httpContext.Response.Cookies.Delete("authToken");
    return Results.Ok(new { message = "Logout successful" });
});

app.MapPost("/api/recover-password", async (IUserService userService, PasswordRecoveryDto recoveryDto) =>
{
    var result = await userService.RecoverPasswordAsync(recoveryDto);
    return Results.Ok("If a user with that email exists, a password recovery email has been sent.");
});

app.Run();