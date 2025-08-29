using Dapper;
using ITPLibrary.Api.Core.Services;
using ITPLibrary.Api.Data.Entities;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IDbConnection>(sp => new SqlConnection(
    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IBookService, BookService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/minimal/books", async (IDbConnection db) =>
{
    var sql = "SELECT * FROM Books";
    var books = await db.QueryAsync<Book>(sql);
    return Results.Ok(books);
});

app.MapPost("/api/minimal/books", async (IDbConnection db, Book book) =>
{
    var sql = "INSERT INTO Books (Title, Author, Genre) VALUES (@Title, @Author, @Genre)";
    await db.ExecuteAsync(sql, new { book.Title, book.Author, book.Genre });
    return Results.Created($"/api/minimal/books/{book.Id}", book);
});

app.Run();