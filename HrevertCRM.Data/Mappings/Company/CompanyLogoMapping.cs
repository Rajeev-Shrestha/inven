using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class CompanyLogoMapping
    {
        public CompanyLogoMapping(EntityTypeBuilder<CompanyLogo> entityBuilder)
        {
            entityBuilder.HasKey(x=>x.Id);
            entityBuilder.Property(x=>x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(x=>x.CreatedByUser)
                .WithMany(x=>x.CompanyLogoCreated)
                .HasForeignKey(x=>x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.CompanyLogoModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}
