using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class DiscountMapping
    {

        public DiscountMapping(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.DiscountCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LastUser)
                .WithMany(x => x.DiscountModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CustomerLevel)
                .WithMany(x => x.Discounts)
                .HasForeignKey(x => x.CustomerLevelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
