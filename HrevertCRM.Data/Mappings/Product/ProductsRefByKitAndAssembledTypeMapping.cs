using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ProductsRefByKitAndAssembledTypeMapping
    {
        public ProductsRefByKitAndAssembledTypeMapping(EntityTypeBuilder<ProductsRefByKitAndAssembledType> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ProductsRefByKitAndAssembledTypeModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ProductsRefByKitAndAssembledTypeCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Product)
                .WithMany(x => x.ProductsReferencedByKitAndAssembledTypes)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
