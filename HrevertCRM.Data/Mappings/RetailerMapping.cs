using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data
{
    public class RetailerMapping
    {
        public RetailerMapping(EntityTypeBuilder<Retailer> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.RetailerCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.RetailerModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
