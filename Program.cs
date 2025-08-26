using Microsoft.EntityFrameworkCore;
using TodoApi.DatabaseContext; // Your DbContext namespace
using TodoApi.Extensions;
using TodoApi.Mapping; // AutoMapper profiles namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add DbContext with SQL Server
builder.Services.AddDatabase(builder.Configuration); // extension method from DatabaseServiceExtensions

// Add Swagger
builder.Services.AddSwaggerDocumentation(); // extension method from SwaggerServiceExtensions

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(TodoProfile));

// Add repository / unit-of-work / service DI here if needed
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Apply migrations and seed database
await app.Services.SeedDatabaseAsync();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable Swagger UI at root
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1");
    c.RoutePrefix = string.Empty; // Swagger at root: https://localhost:5001/
});

app.UseHttpsRedirection();

app.UseAuthorization(); // no JWT, just keep Authorization middleware

app.MapControllers();

app.Run();