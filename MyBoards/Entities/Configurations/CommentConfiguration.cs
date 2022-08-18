using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> eb)
        {
            eb.Property(c => c.CreatedDate).HasDefaultValueSql("getutcdate()");
            eb.Property(x => x.UpdatedDate).ValueGeneratedOnUpdate();
            eb.HasOne(u => u.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(f => f.AuthorId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
