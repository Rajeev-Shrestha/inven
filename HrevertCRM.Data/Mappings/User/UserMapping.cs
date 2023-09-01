using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class UserMapping
    {
        public UserMapping(EntityTypeBuilder<ApplicationUser> entityBuilder)
        {
            // entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            //entityBuilder.Property(t => t.FirstName).IsRequired();
            //entityBuilder.Property(t => t.LastName).IsRequired();
            //entityBuilder.Property(t => t.Username).IsRequired();
            //entityBuilder.Property(t => t.Password).IsRequired();
            entityBuilder.Property(t => t.Email).IsRequired();
            //entityBuilder.Property(t => t.Gender).IsRequired();
            //entityBuilder.Property(t => t.Address).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.UsersModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.UsersCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}