using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class CompanyWebSettingMapping
    {
        public CompanyWebSettingMapping(EntityTypeBuilder<CompanyWebSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            builder.HasOne(x => x.LastUser)
                .WithMany(x => x.CompanyWebSettingModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.CompanyWebSettingCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
