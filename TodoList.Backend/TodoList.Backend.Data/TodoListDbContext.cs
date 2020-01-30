using Microsoft.EntityFrameworkCore;
using TodoList.Backend.Models.Database;

namespace TodoList.Backend.Data
{
    public class TodoListDbContext : DbContext
    {

        public TodoListDbContext()
        { }

        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<List> Lists { get; set; }

        public virtual DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
