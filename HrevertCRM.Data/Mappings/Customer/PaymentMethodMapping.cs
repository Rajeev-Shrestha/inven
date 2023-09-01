using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class PaymentMethodMapping
    {
        public PaymentMethodMapping(EntityTypeBuilder<PaymentMethod> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();

            entityBuilder.Property(x => x.MethodCode).IsRequired();
            entityBuilder.Property(x => x.MethodName).IsRequired();
            //entityBuilder.Property(x => x.AccountId).IsRequired(); //For Future

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.PaymentMethodCreated)
                .HasForeignKey(f => f.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.PaymentMethodModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}