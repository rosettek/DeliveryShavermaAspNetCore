﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.DataAccess.Entities;
using OrderService.Domain.Models;

namespace OrderService.DataAccess.Configurations
{
    public class LastOrderConfiguration : IEntityTypeConfiguration<LastOrderEntity>
    {
        public void Configure(EntityTypeBuilder<LastOrderEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(b => b.ClientId)
                .IsRequired();

            builder.Property(b => b.CourierId)
                .IsRequired();

            builder.Property(b => b.StoreId)
                .IsRequired();

            builder.Property(b => b.Basket)
                .HasColumnType("jsonb")
                .IsRequired();


            builder.Property(b => b.Price)
                .IsRequired();

            builder.Property(b => b.Comment)
                .HasMaxLength(OrderBase.MaxCommentLength);

            builder.Property(b => b.CourierNumber)
                .HasMaxLength(OrderBase.MaxNumberLength)
                .IsRequired();

            builder.Property(b => b.ClientNumber)
                .HasMaxLength(OrderBase.MaxCommentLength)
                .IsRequired();

            builder.Property(b => b.ClientAddress)
                .HasMaxLength(OrderBase.MaxAddressLength)
                .IsRequired();

            //cheque grade
        }
    }
}
