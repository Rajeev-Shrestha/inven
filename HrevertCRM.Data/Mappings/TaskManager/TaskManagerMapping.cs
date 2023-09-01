using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HrevertCRM.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HrevertCRM.Data.Mappings
{
    public class TaskManagerMapping
    {
        public TaskManagerMapping(EntityTypeBuilder<TaskManager> entityBuilder)
        {
            entityBuilder.HasKey(key => key.TaskId);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(x => x.TaskTitle).IsRequired();
            entityBuilder.Property(x => x.TaskDescription).IsRequired();
            entityBuilder.Property(x => x.TaskStartDateTime).IsRequired();
            entityBuilder.Property(x => x.TaskEndDateTime).IsRequired();
            entityBuilder.Property(x => x.TaskPriority).IsRequired();
            entityBuilder.Property(x => x.Status).IsRequired();
            entityBuilder.Property(x => x.CompletePercentage).IsRequired();
            entityBuilder.Property(x => x.TaskAssignedToUser).IsRequired();

            entityBuilder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.TasksAssignedToUser)
                .HasForeignKey(x => x.TaskAssignedToUser)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.CreatedByUser)
                .WithMany(x => x.TaskManagerCreated)
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(x => x.LastUser)
                .WithMany(x => x.TaskManagerModified)
                .HasForeignKey(x => x.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);




        }
    }
}
