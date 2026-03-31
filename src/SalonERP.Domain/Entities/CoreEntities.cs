using SalonERP.Domain.Common;
using SalonERP.Domain.Enums;

namespace SalonERP.Domain.Entities;

public sealed class Company : AuditableEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? CommercialRegistrationNo { get; set; }
    public string? TaxRegistrationNo { get; set; }
}

public sealed class Branch : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? AddressAr { get; set; }
    public string? AddressEn { get; set; }
    public string? Phone { get; set; }
}

public sealed class Permission : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
}

public sealed class Customer : AuditableEntity
{
    public string FullNameAr { get; set; } = string.Empty;
    public string FullNameEn { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? WhatsAppNumber { get; set; }
    public string? Email { get; set; }
    public string? Notes { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int LoyaltyPoints { get; set; }
    public decimal TotalSpend { get; set; }
    public int TotalVisits { get; set; }
    public DateTime? LastVisitAtUtc { get; set; }
}

public sealed class Employee : AuditableEntity
{
    public string EmployeeNo { get; set; } = string.Empty;
    public string FullNameAr { get; set; } = string.Empty;
    public string FullNameEn { get; set; } = string.Empty;
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public bool IsSpecialist { get; set; }
}

public sealed class ServiceCategory : AuditableEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
}

public sealed class Service : AuditableEntity
{
    public Guid ServiceCategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public decimal Price { get; set; }
    public decimal? Cost { get; set; }
    public GenderApplicability GenderApplicability { get; set; } = GenderApplicability.Both;
    public bool RequiresSpecialist { get; set; }
    public bool RequiresStation { get; set; }
    public bool CanBeBookedOnline { get; set; } = true;
    public int PreparationMinutes { get; set; }
    public int CleanupMinutes { get; set; }
    public int BufferMinutes { get; set; }
}

public sealed class ProductCategory : AuditableEntity
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
}

public sealed class Product : AuditableEntity
{
    public Guid ProductCategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public decimal ReorderLevel { get; set; }
}

public sealed class Appointment : AuditableEntity
{
    public Guid CustomerId { get; set; }
    public Guid? SpecialistEmployeeId { get; set; }
    public DateTime StartAtUtc { get; set; }
    public DateTime EndAtUtc { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Confirmed;
    public string Source { get; set; } = "Reception";
    public string? Notes { get; set; }
    public ICollection<AppointmentItem> Items { get; set; } = new List<AppointmentItem>();
}

public sealed class AppointmentItem : AuditableEntity
{
    public Guid AppointmentId { get; set; }
    public Guid ServiceId { get; set; }
    public Guid? SpecialistEmployeeId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal UnitPrice { get; set; }
}

public sealed class Invoice : AuditableEntity
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public Guid? AppointmentId { get; set; }
    public Guid? CustomerId { get; set; }
    public string CashierUserId { get; set; } = string.Empty;
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Issued;
    public decimal SubTotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal TaxTotal { get; set; }
    public decimal NetTotal { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal DueAmount { get; set; }
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}

public sealed class InvoiceItem : AuditableEntity
{
    public Guid InvoiceId { get; set; }
    public InvoiceItemType ItemType { get; set; }
    public Guid ReferenceId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal NetAmount { get; set; }
}

public sealed class Payment : AuditableEntity
{
    public Guid InvoiceId { get; set; }
    public string MethodCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime PaidAtUtc { get; set; }
    public string? ReferenceNo { get; set; }
}

public sealed class CashShift : AuditableEntity
{
    public string CashierUserId { get; set; } = string.Empty;
    public DateTime OpenedAtUtc { get; set; }
    public DateTime? ClosedAtUtc { get; set; }
    public decimal OpeningBalance { get; set; }
    public decimal ExpectedClosingBalance { get; set; }
    public decimal? ActualClosingBalance { get; set; }
    public decimal? Variance { get; set; }
    public bool IsClosed { get; set; }
}
