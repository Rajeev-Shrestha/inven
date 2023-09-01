using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ItemMeasureMapping
    {
        public ItemMeasureMapping(EntityTypeBuilder<ItemMeasure> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ItemMeasureModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ItemMeasureCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.Product)
                 .WithMany(t => t.ItemMeasures)
                 .HasForeignKey(d => d.ProductId)
                 .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.MeasureUnit)
                 .WithMany(t => t.ItemMeasures)
                 .HasForeignKey(d => d.MeasureUnitId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
