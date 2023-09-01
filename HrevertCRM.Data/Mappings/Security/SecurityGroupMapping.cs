using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SecurityGroupMapping
    {
        public SecurityGroupMapping(EntityTypeBuilder<SecurityGroup> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.GroupName).IsRequired();
            entityBuilder.Property(x => x.GroupDescription).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.SecurityGroupModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.SecurityGroupCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}