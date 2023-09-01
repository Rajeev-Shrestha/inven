using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SecurityRightMapping
    {
        public SecurityRightMapping(EntityTypeBuilder<SecurityRight> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.Allowed).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.SecurityRightModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.SecurityRightCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.SecurityGroup)
                .WithMany(t => t.Rights)
                .HasForeignKey(x => x.SecurityGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            //Member user mapping need to done
            entityBuilder.HasOne(t => t.MemberUser)
                .WithMany(t => t.SecurityRightMemberUsers)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}