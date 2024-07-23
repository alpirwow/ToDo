using CommonLayer.Models.Entity;
using DataLayer.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Contexts
{
    internal class PostgreSQLContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PriorityEntity> Priorities { get; set; }
        public DbSet<ToDoItemEntity> ToDoList { get; set; }

        public PostgreSQLContext()
        {
        }

        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PriorityConfiguration());
            modelBuilder.ApplyConfiguration(new ToDoItemConfiguration());
        }
    }
}
