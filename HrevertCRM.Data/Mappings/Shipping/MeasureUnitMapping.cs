using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class MeasureUnitMapping
    {
        public MeasureUnitMapping(EntityTypeBuilder<MeasureUnit> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.MeasureUnitModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.MeasureUnitCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
