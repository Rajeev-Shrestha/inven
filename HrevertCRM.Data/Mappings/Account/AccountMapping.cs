using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class AccountMapping
    {
        public AccountMapping(EntityTypeBuilder<Entities.Account> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.AccountCode).IsRequired();

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.AccountCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.AccountModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.ParentAccount)
                .WithMany()
                .HasForeignKey(d => d.ParentAccountId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
