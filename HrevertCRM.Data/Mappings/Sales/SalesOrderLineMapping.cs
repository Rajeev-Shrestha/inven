using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SalesOrderLineMapping
    {
        public SalesOrderLineMapping(EntityTypeBuilder<SalesOrderLine> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(x => x.TaxType)
                .WithMany(x => x.SalesOrderLines)
                .HasForeignKey(x => x.TaxId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.SalesOrder)
                .WithMany(x => x.SalesOrderLines)
                .HasForeignKey(x => x.SalesOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Product)
                .WithMany(x => x.SalesOrderLines)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
