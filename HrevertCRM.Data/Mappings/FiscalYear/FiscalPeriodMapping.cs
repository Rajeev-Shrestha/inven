using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class FiscalPeriodMapping
    {
        public FiscalPeriodMapping(EntityTypeBuilder<FiscalPeriod> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.StartDate).IsRequired();
            entityBuilder.Property(x => x.EndDate).IsRequired();

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.FiscalPeriodCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.FiscalPeriodModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Company)
                .WithMany(x => x.FiscalPeriods)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.FiscalYear)
                .WithMany(x => x.FiscalPeriods)
                .HasForeignKey(x => x.FiscalYearId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}