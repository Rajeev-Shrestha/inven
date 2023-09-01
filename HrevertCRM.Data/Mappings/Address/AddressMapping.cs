using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class AddressMapping
    {
        public AddressMapping(EntityTypeBuilder<Address> entityBuilder)
        {
            entityBuilder.HasKey(key => key.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.FirstName).IsRequired();
            entityBuilder.Property(x => x.LastName).IsRequired();
            entityBuilder.Property(x => x.Email).IsRequired();
            entityBuilder.Property(x => x.MobilePhone).IsRequired();

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.AddressCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.AddressModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.User)
                .WithMany(x => x.UserAddresses)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Company)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Customer)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Vendor)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.VendorId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.DeliveryZone)
                .WithMany(x => x.Addresses)
                .HasForeignKey(x => x.DeliveryZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}