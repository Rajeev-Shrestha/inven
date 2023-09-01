using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.ThemeSettings
{
    public class LayoutSettingMapping
    {
        public LayoutSettingMapping(EntityTypeBuilder<LayoutSetting> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.LayoutSettingCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.LayoutSettingModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
