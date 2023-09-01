using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ProductInCategoryMapping
    {
        public ProductInCategoryMapping(EntityTypeBuilder<ProductInCategory> entityBuilder)
        {
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.HasKey(t => new {t.ProductId, t.CategoryId});

            entityBuilder.Ignore(t => t.Id);

            //Many to Many Mapping between Product and ProductCategory
            entityBuilder.HasOne(p => p.Product)
                .WithMany(pin => pin.ProductInCategories)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(pc => pc.ProductCategory)
                .WithMany(pin => pin.ProductInCategories)
                .HasForeignKey(pt => pt.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ProductInCategoriesCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.ProductInCategoriesModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}