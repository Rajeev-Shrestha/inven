using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class CustomerMapping
    {
        public CustomerMapping(EntityTypeBuilder<Entities.Customer> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Ignore(x => x.ConfirmPassword);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CustomerCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
          
            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.CustomerModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.CustomerLevel)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.CustomerLevelId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PaymentTerm)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.PaymentTermId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.PaymentMethod)
                .WithMany(x => x.Customers)
                .HasForeignKey(x => x.PaymentMethodId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}