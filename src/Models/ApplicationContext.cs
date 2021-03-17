using Microsoft.EntityFrameworkCore;

namespace EfSamples.Model
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            if (!Database.CanConnect())
            {
                Database.EnsureCreated();   // создаем базу данных при первом обращении
            }
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey
                (x => x.Id);

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Email = "email@ggg.com", Login = "User"},
                new User() { Id = 2, Email = "2email@ggg.com", Login = "User1"},
                new User() { Id = 3, Email = "3email@ggg.com", Login = "User2"},
                new User() { Id = 4, Email = "4email@ggg.com", Login = "User3"}
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
