using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SecurityMapping
    {
        public SecurityMapping(EntityTypeBuilder<Security> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Ignore(p => p.CompanyId);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.SecurityCode).IsRequired();
            entityBuilder.Property(x => x.SecurityDescription).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.SecurityModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.SecurityCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasMany(t => t.SecurityRights)
                .WithOne(t => t.Security)
                .HasForeignKey(x => x.SecurityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}