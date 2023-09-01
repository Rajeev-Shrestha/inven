using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.ThemeSettings
{
    public class FooterSettingMapping
    {
        public FooterSettingMapping(EntityTypeBuilder<FooterSetting> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.FooterSettingCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.FooterSettingModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
