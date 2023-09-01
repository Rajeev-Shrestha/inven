using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class TaxMapping
    {
        public TaxMapping(EntityTypeBuilder<Tax> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.TaxCode).IsRequired();

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.TaxCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.TaxModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
