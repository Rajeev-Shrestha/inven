using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class DeliveryZoneMapping
    {
        public DeliveryZoneMapping(EntityTypeBuilder<DeliveryZone> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityTypeBuilder.Property(x => x.ZoneName).IsRequired();
            entityTypeBuilder.Property(x => x.ZoneCode).IsRequired();

            entityTypeBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.DeliveryZoneModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.DeliveryZoneCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
