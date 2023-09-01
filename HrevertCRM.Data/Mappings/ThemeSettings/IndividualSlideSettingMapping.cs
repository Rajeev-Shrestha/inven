using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings.ThemeSettings
{
    public class IndividualSlideSettingMapping
    {
        public IndividualSlideSettingMapping(EntityTypeBuilder<IndividualSlideSetting> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityTypeBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.IndividualSlideSettingCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.IndividualSlideSettingModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(x => x.SlideSetting)
                .WithMany(x => x.IndividualSlideSettings)
                .HasForeignKey(x => x.SlideSettingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
