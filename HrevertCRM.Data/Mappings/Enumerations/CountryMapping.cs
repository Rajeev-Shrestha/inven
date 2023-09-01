using HrevertCRM.Entities.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.Enumerations
{
    internal class CountryMapping
    {
        public CountryMapping(EntityTypeBuilder<Country> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(t => t.Name).IsRequired();
            entityBuilder.Property(t => t.Code).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.CountryModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.CountryCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

