using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class DeliveryRateMapping
    {
        public DeliveryRateMapping(EntityTypeBuilder<DeliveryRate> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.DeliveryRateModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.DeliveryRateCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.DeliveryZone)
                .WithMany(x => x.DeliveryRates)
                .HasForeignKey(x => x.DeliveryZoneId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.DeliveryMethod)
                 .WithMany(x => x.DeliveryRates)
                 .HasForeignKey(x => x.DeliveryMethodId)
                 .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.MeasureUnit)
                 .WithMany(x => x.DeliveryRates)
                 .HasForeignKey(x => x.MeasureUnitId)
                 .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.Product)
                 .WithMany(x => x.DeliveryRates)
                 .HasForeignKey(x => x.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.DeliveryRates)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
