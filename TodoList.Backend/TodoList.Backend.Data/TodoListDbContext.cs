using Microsoft.EntityFrameworkCore;
using System;
using TodoList.Backend.Models.Database;

namespace TodoList.Backend.Data
{
    public class TodoListDbContext : DbContext
    {
        public TodoListDbContext(DbContextOptions<TodoListDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }

        public DbSet<Models.Database.TodoList> Lists { get; set; }

        public DbSet<TodoItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relationships
            modelBuilder.Entity<User>()
                .HasMany(user => user.Lists)
                .WithOne(list => list.User);
            
            modelBuilder.Entity<Models.Database.TodoList>()
                .HasOne(list => list.User)
                .WithMany(user => user.Lists);

            modelBuilder.Entity<Models.Database.TodoList>()
                .HasMany(list => list.Items)
                .WithOne(items => items.List);
            
            modelBuilder.Entity<TodoItem>()
                .HasOne(item => item.List)
                .WithMany(list => list.Items);
            

            //Properties and indexes

            //User
            modelBuilder.Entity<User>()
                .Property(user => user.Email)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.FirstName)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.LastName)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.UniqueToken)
                .HasMaxLength(60)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(user => user.IsAdmin)
                .IsRequired();
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique()
                .HasName("Unique_Index_User_Id");
            
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UniqueToken)
                .IsUnique()
                .HasName("Unique_Index_User_UniqueToken");
            
            //List
            modelBuilder.Entity<Models.Database.TodoList>()
                .Property(list => list.Title)
                .IsRequired();

            modelBuilder.Entity<Models.Database.TodoList>()
                .HasIndex(u => u.Id)
                .IsUnique()
                .HasName("Unique_Index_TodoList_Id");
            
            modelBuilder.Entity<Models.Database.TodoList>()
                .HasIndex(u => u.Title)
                .HasName("Index_TodoList_Title");
            
            //Item
            modelBuilder.Entity<TodoItem>()
                .Property(item => item.Description)
                .IsRequired();

            modelBuilder.Entity<TodoItem>()
                .Property(item => item.IsShared)
                .IsRequired();

            modelBuilder.Entity<TodoItem>()
                .Property(item => item.Description)
                .IsRequired();

            modelBuilder.Entity<TodoItem>()
                .HasIndex(u => u.Id)
                .IsUnique()
                .HasName("Unique_Index_TodoItem_Id");

            modelBuilder.Entity<TodoItem>()
                .HasIndex(u => u.Description)
                .HasName("Index_TodoItem_Description");
        }
    }
}
