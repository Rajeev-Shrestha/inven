﻿using HrevertCRM.Entities.Enumerations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrevertCRM.Data.Mappings.Enumerations
{
    internal class ImageTypeMapping
    {
        public ImageTypeMapping(EntityTypeBuilder<ImageTypes> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
            entityBuilder.Property(t => t.Value).IsRequired();

            entityBuilder.HasOne(t => t.LastUser)
                .WithMany(t => t.ImageTypeModified)
                .HasForeignKey(d => d.LastUpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder.HasOne(t => t.CreatedByUser)
                .WithMany(t => t.ImageTypeCreated)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
