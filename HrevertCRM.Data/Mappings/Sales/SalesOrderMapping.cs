using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SalesOrderMapping
    {
        public SalesOrderMapping(EntityTypeBuilder<SalesOrder> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
          //  entityBuilder.Property(x => x.PurchaseOrderNumber).IsRequired();
            entityBuilder.Property(x => x.DueDate).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.SalesOrderModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.SalesOrderCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.SalesRep)
                .WithMany(t => t.SalesOrdersBySalesRep)
                .HasForeignKey(d => d.SalesRepId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.FiscalPeriod)
                .WithMany(t => t.SalesOrders)
                .HasForeignKey(x => x.FiscalPeriodId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Customer)
                .WithMany(t => t.SalesOrders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.DeliveryMethod)
                .WithMany(x => x.SalesOrders)
                .HasForeignKey(x => x.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PaymentTerm)
                .WithMany(x => x.SalesOrders)
                .HasForeignKey(x => x.PaymentTermId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PaymentMethod)
                .WithMany(x => x.SalesOrders)
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}