using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DarkStrollsAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace DarkStrollsAPI.Models
{
    /// <summary>
    /// Database context for the API.
    /// </summary>
    public class DarkDbContext : DbContext
    {
        /// <summary>
        /// Connection string, loaded at Startup.
        /// </summary>
        public static string? ConnectionString { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public DarkDbContext() : base()
        {
        }

        /// <summary>
        /// DbSet for messages in the database.
        /// </summary>
        public DbSet<Message> Messages { get; set; } = null!;

        /// <summary>
        /// DbSet for users in the database.
        /// </summary>
        public DbSet<User> Users { get; set; } = null!;
        
        /// <summary>
        /// DbSet for bonfires in the database.
        /// </summary>
        public DbSet<Bonfire> Bonfires { get; set; } = null!;

        /// <summary>
        /// Called when initializing this context.
        /// Links entities to tables.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().ToTable("messages");
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Bonfire>().ToTable("bonfires");
        }

        /// <summary>
        /// Called when creating with the default constructor.
        /// Sets up connection string.
        /// </summary>
        /// <param name="optionsBuilder">DbContextOptionsBuilder to create options with.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString);
        }
    }
}
