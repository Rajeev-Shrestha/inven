using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class CustomerInContactGroupMapping
    {
        public CustomerInContactGroupMapping(EntityTypeBuilder<CustomerInContactGroup> entityBuilder)
        {
            //Many to Many Mapping between Customer and CustomerContactGroup
            entityBuilder.HasKey(t => new {t.CustomerId, t.ContactGroupId});
            entityBuilder.Ignore(t => t.Id);

            entityBuilder.HasOne(c => c.Customer)
                .WithMany(cic => cic.CustomerInContactGroups)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.ContactGroup)
                .WithMany(x => x.CustomerAndContactGroupList)
                .HasForeignKey(x => x.ContactGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}