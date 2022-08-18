using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
           builder.Property(wi => wi.Activity)
                .HasMaxLength(200);

           builder.Property(wi => wi.RemainingWork)
                .HasPrecision(14, 2);

        }
    }
}
