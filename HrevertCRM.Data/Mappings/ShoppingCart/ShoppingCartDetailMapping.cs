using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ShoppingCartDetailMapping
    {
        public ShoppingCartDetailMapping(EntityTypeBuilder<ShoppingCartDetail> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(x => x.CreatedByUser)
              .WithMany(x => x.ShoppingCartDetailCreated)
              .HasForeignKey(x => x.CreatedBy)
              .OnDelete(DeleteBehavior.Restrict);
           
            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.ShoppingCartDetailModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Product)
                .WithMany(x => x.ShoppingCartDetails)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.ShoppingCart)
                .WithMany(x => x.ShoppingCartDetails)
                .HasForeignKey(x => x.ShoppingCartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
