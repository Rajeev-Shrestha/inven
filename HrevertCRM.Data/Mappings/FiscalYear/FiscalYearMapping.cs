using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class FiscalYearMapping
    {
        public FiscalYearMapping(EntityTypeBuilder<FiscalYear> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.StartDate).IsRequired();
            entityBuilder.Property(x => x.EndDate).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.FiscalYearModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.FiscalYearCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Company)
                .WithMany(t => t.FiscalYears)
                .HasForeignKey(t => t.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}