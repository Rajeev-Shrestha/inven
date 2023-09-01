using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SecurityGroupMemberMapping
    {
        public SecurityGroupMemberMapping(EntityTypeBuilder<SecurityGroupMember> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.SecurityGroupMemberModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.SecurityGroupMemberCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.SecurityGroup)
                .WithMany(t => t.Members)
                .HasForeignKey(x => x.SecurityGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.MemberUser)
                .WithMany(t => t.SecurityGroupMemberUsers)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}