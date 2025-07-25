using Microsoft.EntityFrameworkCore;

namespace TodoProject.Entities
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }     

        public DbSet<User> Users { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<UserNote> UserNotes { get; set; }

    }
}
