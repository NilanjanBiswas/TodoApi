using Microsoft.AspNetCore.Identity;
using TodoApi.Models;
using TodoApi.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Extensions
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _db;
        public DbSeeder(ApplicationDbContext db) => _db = db;


        public async Task SeedAsync()
        {
            // if users exist, assume seeded
            if (await _db.Users.AnyAsync()) return;


            // create a test user with hashed password
            var user = new User
            {
                UserName = "demo_user",
                Email = "demo@example.com",
            };


            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, "Demo@123");


            _db.Users.Add(user);


            // add some todos for the seeded user
            var todos = new[]
            {
                new TodoItem { Title = "Welcome to TodoApp", Description = "This is your first todo.", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(7), User = user },
                new TodoItem { Title = "Learn the API", Description = "Try signup, login and call protected endpoints.", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(3), User = user },
                new TodoItem { Title = "Demo completed", Description = "Mark this todo as completed.", IsCompleted = true, CreatedAt = DateTime.UtcNow.AddDays(-1), User = user }
                };


            _db.Todos.AddRange(todos);


            await _db.SaveChangesAsync();
        }
    }
}
