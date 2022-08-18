using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities.Configurations
{
    public class WorkItemConfiguration : IEntityTypeConfiguration<WorkItem>
    {
        public void Configure(EntityTypeBuilder<WorkItem> eb)
        {
            eb.HasOne(wi => wi.State)
                .WithMany()
                .HasForeignKey(wi => wi.StateId);

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
                .HasForeignKey(wit => wit.TagId),
                wi => wi.HasOne(wit => wit.WorkItem)
                .WithMany()
                .HasForeignKey(wit => wit.WorkItemId),
                wi =>
                {
                    wi.HasKey(x => new { x.TagId, x.WorkItemId });
                    wi.Property(x => x.PublicationDate).HasDefaultValueSql("getutcdate()");
                }
                );
        }
    }
}
