namespace SalonERP.Application.DTOs;

public sealed class CreateInvoiceDto
{
    public Guid BranchId { get; set; }
    public Guid? CustomerId { get; set; }
    public Guid? AppointmentId { get; set; }
    public string CashierUserId { get; set; } = string.Empty;
    public decimal DiscountAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
    public List<CreatePaymentDto> Payments { get; set; } = new();
}

public sealed class CreateInvoiceItemDto
{
    public int ItemType { get; set; }
    public Guid ReferenceId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public sealed class CreatePaymentDto
{
    public string MethodCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? ReferenceNo { get; set; }
}
