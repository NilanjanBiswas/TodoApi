using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApi.DatabaseContext; // Your DbContext namespace
using TodoApi.Extensions;     // Your service extension methods
using TodoApi.Mapping;
using TodoApi.Repositories;
using TodoApi.Service;        // AutoMapper profiles namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioningExtension(); // api versioning setup
builder.Services.AddSwaggerDocumentation(); // swagger setup


// Add DbContext with SQL Server
builder.Services.AddDatabase(builder.Configuration); // extension method from DatabaseServiceExtensions



//validation

// Register FluentValidation validators automatically
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


// Add AutoMapper (scan profiles)
builder.Services.AddAutoMapper(typeof(TodoProfile).Assembly);

// 🔹 Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("SecretKey not configured"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// 🔹 Add Authorization
builder.Services.AddAuthorization();

// Add repository / unit-of-work / service DI here if needed
// builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Repository / service DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITodoService, TodoService>();


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
app.UseSwaggerUI();

app.UseHttpsRedirection();

// 🔹 Use Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
