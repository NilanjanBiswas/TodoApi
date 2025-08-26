using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.DatabaseContext 
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }


        public DbSet<User> Users { get; set; } = default!;
        public DbSet<TodoItem> Todos { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(u => u.Email).IsUnique();
                b.Property(u => u.Email).IsRequired().HasMaxLength(256);
                b.Property(u => u.UserName).IsRequired().HasMaxLength(100);
                b.Property(u => u.PasswordHash).IsRequired();
            });


            modelBuilder.Entity<TodoItem>(b =>
            {
                b.Property(t => t.Title).IsRequired().HasMaxLength(250);
                b.Property(t => t.Description).HasMaxLength(2000);
                b.HasOne(t => t.User)
                .WithMany(u => u.Todos)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
