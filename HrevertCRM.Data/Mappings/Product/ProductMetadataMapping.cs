using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class ProductMetadataMapping
    {
        public ProductMetadataMapping(EntityTypeBuilder<ProductMetadata> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Property(x => x.MediaType).IsRequired();
            entityBuilder.Property(x => x.MediaUrl).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ProductMetadataModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ProductMetadataCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            //One to Many Mapping Between Product and ProductMetadata
            entityBuilder.HasOne(p => p.Product)
                .WithMany(i => i.ProductMetadatas)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}