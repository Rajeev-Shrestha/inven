using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    internal class CompanyMapping
    {
        public CompanyMapping(EntityTypeBuilder<Company> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

            entityBuilder.Ignore(e => e.CompanyId);
            //entityBuilder.Property(t => t.Name).IsRequired();
            //entityBuilder.Property(t => t.Address).IsRequired();
            //entityBuilder.Property(t => t.Email).IsRequired();
            //entityBuilder.Property(t => t.PhoneNumber).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.CompaniesModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.CompaniesCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasMany(x => x.SalesManagers)
                .WithOne(t => t.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasMany(x => x.SalesOrders)
                .WithOne(t => t.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasMany(x => x.PurchaseOrders)
                .WithOne(t => t.Company)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}