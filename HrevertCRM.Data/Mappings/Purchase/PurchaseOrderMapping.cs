using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class PurchaseOrderMapping
    {
        public PurchaseOrderMapping(EntityTypeBuilder<PurchaseOrder> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
          //  entityBuilder.Property(x => x.SalesOrderNumber).IsRequired();
            entityBuilder.Property(x => x.OrderDate).IsRequired();
            entityBuilder.Property(x => x.DueDate).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.PurchaseOrderModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.PurchaseOrderCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.PurchaseRep)
                .WithMany(t => t.PurchaseOrderByPurchaseRep)
                .HasForeignKey(d => d.PurchaseRepId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.FiscalPeriod)
                .WithMany(t => t.PurchaseOrders)
                .HasForeignKey(x => x.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.DeliveryMethod)
                .WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PaymentTerm)
                .WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.PaymentTermId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Vendor)
                 .WithMany(x => x.PurchaseOrders)
                 .HasForeignKey(x => x.VendorId)
                 .OnDelete(DeleteBehavior.Restrict);
        }
    }
}