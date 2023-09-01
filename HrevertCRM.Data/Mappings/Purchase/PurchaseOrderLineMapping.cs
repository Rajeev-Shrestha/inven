using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class PurchaseOrderLineMapping
    {
        public PurchaseOrderLineMapping(EntityTypeBuilder<PurchaseOrderLine> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(x => x.TaxType)
                .WithMany(x => x.PurchaseOrderLines)
                .HasForeignKey(x => x.TaxId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PurchaseOrder)
                .WithMany(x => x.PurchaseOrderLines)
                .HasForeignKey(x => x.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Product)
                .WithMany(x => x.PurchaseOrderLines)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
