using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class SalesOpportunityMapping
    {
        public SalesOpportunityMapping(EntityTypeBuilder<SalesOpportunity> entityBuilder)
        {
            entityBuilder.HasKey(key => key.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.Title).IsRequired();
            entityBuilder.Property(x => x.ClosingDate).IsRequired();
            entityBuilder.Property(x => x.BusinessValue).IsRequired();
            entityBuilder.Property(x => x.Probability).IsRequired();
            entityBuilder.Property(x => x.Priority).IsRequired();
            entityBuilder.Property(x => x.ClosedDate).IsRequired(); 

            entityBuilder.HasOne(x => x.Customer)
                 .WithMany(x => x.SalesOpportunity)
                 .HasForeignKey(x => x.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.SalesOpportunity)
                .HasForeignKey(x => x.SalesRepresentative)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Source)
                .WithMany(x => x.SalesOpportunities)
                .HasForeignKey(x => x.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x=>x.Stage)
                  .WithMany(x => x.SalesOpportunities)
                .HasForeignKey(x => x.StageId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.Grade)
                 .WithMany(x => x.SalesOpportunities)
               .HasForeignKey(x => x.GradeId)
               .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.ReasonClosed)
               .WithMany(x => x.SalesOpportunities)
             .HasForeignKey(x => x.ReasonClosedId)
             .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.CreatedByUser)
              .WithMany(x => x.SalesOpportunityCreated)
              .HasForeignKey(x => x.CreatedBy)
              .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.SalesOpportunityModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
