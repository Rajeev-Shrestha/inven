using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SourceMapping
    {
        public SourceMapping(EntityTypeBuilder<Source> entityBuilder)
        {
            entityBuilder.HasKey(key => key.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.SourceName).IsRequired();

            entityBuilder.HasOne(x => x.CreatedByUser)
           .WithMany(x => x.SourceCreated)
           .HasForeignKey(x => x.CreatedBy)
           .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.SourceModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
