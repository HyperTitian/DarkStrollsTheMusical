
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class DSContext : DbContext
{
  public static string ConnectionString { get; set; }

  //public DbSet<User> Users { get; set; }
  
  //public DbSet<Message> Messages { get; set; }

  public DSContext(DbContextOptions options) : base(options)
  {
  
  }
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    //modelBuilder.Entity<User>().ToTable("users");
    //modelBuilder.Entity<Message>().ToTable("messages");
  }

  public DSContext CreateContext()
  {
    var options = new DbContextOptionsBuilder<DSContext>();
    options.UseMySQL(ConnectionString);

    return new DSContext(options.Options);
  }
}
