using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class GradeMapping
    {
        public GradeMapping(EntityTypeBuilder<Grade> entityBuilder)
        {
            entityBuilder.HasKey(key => key.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.GradeName).IsRequired();
            entityBuilder.Property(x => x.Rank).IsRequired();

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.GradeCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.GradeModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
