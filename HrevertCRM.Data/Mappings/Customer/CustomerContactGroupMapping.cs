using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class CustomerContactGroupMapping
    {
        public CustomerContactGroupMapping(EntityTypeBuilder<CustomerContactGroup> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Ignore(x => x.CustomerIdsInContactGroup);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CustomerContactGroupCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.CustomerContactGroupModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}