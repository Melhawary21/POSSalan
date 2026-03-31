using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SalonERP.Domain.Entities;

namespace SalonERP.Infrastructure.Data.Configurations;

public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");
        builder.Property(x => x.Code).HasMaxLength(32).IsRequired();
        builder.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.Property(x => x.FullNameAr).HasMaxLength(250).IsRequired();
        builder.Property(x => x.FullNameEn).HasMaxLength(250).IsRequired();
        builder.Property(x => x.MobileNumber).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.BranchId, x.MobileNumber });
    }
}

public sealed class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");
        builder.Property(x => x.Code).HasMaxLength(64).IsRequired();
        builder.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Cost).HasColumnType("decimal(18,2)");
        builder.HasIndex(x => new { x.BranchId, x.Code }).IsUnique();
    }
}

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.Property(x => x.Code).HasMaxLength(64).IsRequired();
        builder.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
        builder.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CostPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.SellingPrice).HasColumnType("decimal(18,2)");
        builder.Property(x => x.ReorderLevel).HasColumnType("decimal(18,3)");
        builder.HasIndex(x => new { x.BranchId, x.Code }).IsUnique();
    }
}

public sealed class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("Appointments");
        builder.Property(x => x.Source).HasMaxLength(32).IsRequired();
        builder.HasIndex(x => new { x.BranchId, x.StartAtUtc, x.Status });
        builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.AppointmentId);
    }
}

public sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.Property(x => x.InvoiceNumber).HasMaxLength(40).IsRequired();
        builder.Property(x => x.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DiscountTotal).HasColumnType("decimal(18,2)");
        builder.Property(x => x.TaxTotal).HasColumnType("decimal(18,2)");
        builder.Property(x => x.NetTotal).HasColumnType("decimal(18,2)");
        builder.Property(x => x.PaidAmount).HasColumnType("decimal(18,2)");
        builder.Property(x => x.DueAmount).HasColumnType("decimal(18,2)");
        builder.HasIndex(x => x.InvoiceNumber).IsUnique();
        builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.InvoiceId);
    }
}
