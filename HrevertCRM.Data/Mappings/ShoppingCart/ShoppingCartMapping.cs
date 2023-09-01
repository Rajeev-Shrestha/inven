using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HrevertCRM.Data.Mappings
{
    public class ShoppingCartMapping
    {
        public ShoppingCartMapping(EntityTypeBuilder<ShoppingCart> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.HostIp);
            entityBuilder.Property(x => x.Amount).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ShoppingCartModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ShoppingCartCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
