using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.ThemeSettings
{
    public class BrandImageMapping
    {
        public BrandImageMapping(EntityTypeBuilder<BrandImage> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
           .WithMany(x => x.BrandImageCreated)
           .HasForeignKey(x => x.CreatedBy)
           .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.BrandImageModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.FooterSetting)
                .WithMany(x => x.BrandImages)
                .HasForeignKey(x => x.FooterSettingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
