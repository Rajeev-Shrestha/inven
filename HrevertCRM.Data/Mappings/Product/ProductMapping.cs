using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ProductMapping
    {
        public ProductMapping(EntityTypeBuilder<Product> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(t => t.Code).IsRequired();
            entityBuilder.Property(t => t.Name).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ProductsModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ProductsCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many Mapping between Product and ProductPriceRule
            entityBuilder.HasMany(t => t.ProductPriceRules)
                .WithOne(t => t.Product)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many Mapping between Product and ProductPriceRule
            entityBuilder.HasMany(t => t.ProductRates)
                .WithOne(t => t.Product)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many Mapping between Product and Companies
            entityBuilder.HasOne(t => t.Company)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}