using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkStrollsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DarkStrollsAPI.Models
{
    public class DarkDbContext : DbContext
    {
        public static string? ConnectionString { get; set; }

        public DarkDbContext() : base()
        {
        }

        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("messages");
            modelBuilder.Entity<User>().ToTable("users");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
