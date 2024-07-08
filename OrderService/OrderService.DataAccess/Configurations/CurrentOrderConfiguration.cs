﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.DataAccess.Entities;
using OrderService.Domain.Models;


namespace OrderService.DataAccess.Configurations
{
    public class CurrentOrderConfiguration : IEntityTypeConfiguration<CurrentOrderEntity>
    {
        public void Configure(EntityTypeBuilder<CurrentOrderEntity> builder)
        {
            new BaseOrderConfiguration<CurrentOrderEntity>().Configure(builder);

            builder.Property(b => b.Status)
                .IsRequired();

            builder.Property(b => b.StoreAddress)
                .HasMaxLength(BaseOrder.MaxAddressLength)
                .IsRequired();

            builder.Property(b => b.CourierNumber)
                .HasMaxLength(BaseOrder.MaxNumberLength)
                .IsRequired();

            builder.Property(b => b.ClientNumber)
                .HasMaxLength(BaseOrder.MaxCommentLength)
                .IsRequired();

            builder.ToTable(table => 
            {
                table.HasCheckConstraint(
                    "CK_ClientNumber", $"ClientNumber ~ '{BaseOrder.RegexForNumber}'");
                table.HasCheckConstraint(
                    "CK_CourierNumber", $"CourierNumber ~ '{BaseOrder.RegexForNumber}'");
            });
            
            builder.Property(b => b.ClientAddress)
                .HasMaxLength(BaseOrder.MaxAddressLength)
                .IsRequired();
        }
    }
}