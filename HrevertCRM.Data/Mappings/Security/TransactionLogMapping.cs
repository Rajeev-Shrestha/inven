using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class TransactionLogMapping
    {
        public TransactionLogMapping(EntityTypeBuilder<TransactionLog> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.TransactionDate).IsRequired();
            entityBuilder.Property(x => x.ChangedItemId).IsRequired();
            entityBuilder.Property(x => x.ItemType).IsRequired();
            entityBuilder.Property(x => x.Description).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.TransactionLogModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.TransactionLogCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.Security)
                .WithMany(t => t.TransactionLogs)
                .HasForeignKey(x => x.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}