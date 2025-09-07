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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adaug? aceste dou? linii pentru a folosi autentificarea ?i autorizarea
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

app.MapPost("/api/login", async (IUserService userService, LoginUserDto userDto) =>
{
    var token = await userService.LoginUserAsync(userDto);
    if (token == null)
    {
        return Results.Unauthorized();
    }
    return Results.Ok(new { Token = token });
});

app.MapPost("/api/recover-password", async (IUserService userService, PasswordRecoveryDto recoveryDto) =>
{
    var result = await userService.RecoverPasswordAsync(recoveryDto);
    return Results.Ok("If a user with that email exists, a password recovery email has been sent.");
});

app.Run();