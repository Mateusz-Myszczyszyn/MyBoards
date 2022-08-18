using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyBoards.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoards.Entities.Configurations
{
    public class TopAuthorConfiguration : IEntityTypeConfiguration<TopAuthor>
    {
        public void Configure(EntityTypeBuilder<TopAuthor> eb)
        {
            eb.ToView("View_TopAuthors");
            eb.HasNoKey();
        }
    }
}
