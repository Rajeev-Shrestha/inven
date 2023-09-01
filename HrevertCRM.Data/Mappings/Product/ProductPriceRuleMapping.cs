using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ProductPriceRuleMapping
    {
        public ProductPriceRuleMapping(EntityTypeBuilder<ProductPriceRule> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.ProductPriceRuleModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.ProductPriceRuleCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.Product)
                .WithMany(x => x.ProductPriceRules)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.Company)
                .WithMany(x => x.ProductPriceRules)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.Category)
                .WithMany(x => x.ProductPriceRules)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
