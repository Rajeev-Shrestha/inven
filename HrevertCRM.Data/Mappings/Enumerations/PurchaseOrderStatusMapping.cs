using HrevertCRM.Entities.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.Enumerations
{
    internal class PurchaseOrderStatusMapping
    {
        public PurchaseOrderStatusMapping(EntityTypeBuilder<PurchaseOrdersStatus> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(t => t.Value).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.PurchaseOrdersStatusModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.PurchaseOrdersStatusCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
