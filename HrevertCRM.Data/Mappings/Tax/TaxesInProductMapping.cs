using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class TaxesInProductMapping
    {
        public TaxesInProductMapping(EntityTypeBuilder<TaxesInProduct> entityBuilder)
        {
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.HasKey(t => new {t.ProductId, t.TaxId});
            entityBuilder.Ignore(t => t.Id);

            entityBuilder.HasOne(p => p.Product)
                .WithMany(tip => tip.TaxesInProducts)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.Tax)
                .WithMany(x => x.TaxesInProducts)
                .HasForeignKey(x => x.TaxId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.TaxesInProductsCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.TaxesInProductsModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
