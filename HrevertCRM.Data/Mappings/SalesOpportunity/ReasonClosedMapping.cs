using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ReasonClosedMapping
    {
        public ReasonClosedMapping(EntityTypeBuilder<ReasonClosed> entityBuilder)
        {
            entityBuilder.HasKey(key => key.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.Reason).IsRequired();

            entityBuilder.HasOne(x => x.CreatedByUser)
              .WithMany(x => x.ReasonClosedCreated)
              .HasForeignKey(x => x.CreatedBy)
              .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.ReasonClosedModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
