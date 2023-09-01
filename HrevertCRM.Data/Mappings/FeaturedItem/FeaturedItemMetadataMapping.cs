using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class FeaturedItemMetadataMapping
    {
        public FeaturedItemMetadataMapping(EntityTypeBuilder<FeaturedItemMetadata> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.MediaType).IsRequired();
            entityBuilder.Property(x => x.MediaUrl).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.FeaturedItemMetadataModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.FeaturedItemMetadataCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many Mapping Between FeaturedItem and FeaturedItemMetadata

            entityBuilder.HasOne(p => p.FeaturedItem)
                .WithMany(i => i.FeaturedItemMetadatas)
                .HasForeignKey(x => x.FeaturedItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
