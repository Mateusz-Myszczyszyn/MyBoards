using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities
{
    public class MyBoardsContext : DbContext
    {
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            /*modelBuilder.Entity<User>()
                .HasKey(x => new { x.Email, x.LastName });*///this is making a primary key combined of two keys
            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.Property(wi => wi.State).IsRequired();
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(wi => wi.Efford).HasColumnType("decimal(5,2)");
                eb.Property(wi => wi.EndDate).HasPrecision(3);
                eb.Property(wi => wi.Activity).HasMaxLength(200);
                eb.Property(wi => wi.RemaininWork).HasPrecision(14, 2);
            });
                
        }
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
    }
}
