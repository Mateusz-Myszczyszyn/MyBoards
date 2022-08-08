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
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Epic> Epics { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<WorkItemState> WorkItemStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
            /*modelBuilder.Entity<User>()
                .HasKey(x => new { x.Email, x.LastName });*///this is making a primary key combined of two keys
            modelBuilder.Entity<Epic>()
                .Property(wi => wi.EndDate)
                .HasPrecision(3);

            modelBuilder.Entity<Task>()
                .Property(wi => wi.Activity)
                .HasMaxLength(200);

            modelBuilder.Entity<Task>()
                .Property(wi => wi.RemaininWork)
                .HasPrecision(14, 2);

            modelBuilder.Entity<Issue>()
                .Property(wi => wi.Efford)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<WorkItem>(eb =>
            {
                eb.HasOne(wi => wi.State)
                .WithMany()
                .HasForeignKey(wi => wi.StateId);

                eb.Property(wi => wi.State).IsRequired();
                eb.Property(wi => wi.Area).HasColumnType("varchar(200)");
                eb.Property(wi => wi.IterationPath).HasColumnName("Iteration_Path");
                eb.Property(wi => wi.Priority).HasDefaultValue(1);

                eb.HasMany(wi => wi.Comments)
                .WithOne(wi => wi.WorkItem)
                .HasForeignKey(wi => wi.WorkItemId);

                eb.HasOne(wi => wi.Author)
                .WithMany(wi => wi.WorkItems)
                .HasForeignKey(wi => wi.AuthorId);

                eb.HasMany(wi => wi.Tags)
                .WithMany(wi => wi.WorkItems)
                .UsingEntity<WorkItemHasTag>(
                    wi => wi.HasOne(wit => wit.Tag)
                    .WithMany()
                    .HasForeignKey(wit=>wit.TagId),
                    wi => wi.HasOne(wit => wit.WorkItem)
                    .WithMany()
                    .HasForeignKey(wit => wit.WorkItemId),
                    wi =>
                    {
                        wi.HasKey(x => new { x.TagId, x.WorkItemId });
                        wi.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                    }

                    ) ;
            });

            modelBuilder.Entity<Comment>(eb =>
            {
                eb.Property(c => c.CreatedDate).HasDefaultValueSql("getutcdate()");
                eb.Property(c => c.UpdatedDate).ValueGeneratedOnUpdate();
            });

            modelBuilder.Entity<User>()
                .HasOne(u => u.Adress)
                .WithOne(u => u.User)
                .HasForeignKey<Address>(a => a.UserId);

            modelBuilder.Entity<WorkItemState>(wis=>
                wis.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(50));

        }
        public MyBoardsContext(DbContextOptions<MyBoardsContext> options) : base(options)
        {

        }
    }
}
