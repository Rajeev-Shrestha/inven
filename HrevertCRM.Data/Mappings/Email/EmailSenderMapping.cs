using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class EmailSenderMapping
    {
        public EmailSenderMapping(EntityTypeBuilder<EmailSender> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.EmailSenderCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.EmailSenderModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.Property(x => x.Subject).IsRequired();
            entityBuilder.Property(x => x.Message).IsRequired();
            entityBuilder.Ignore(x => x.Files);
        }
    }
}
