namespace SalonERP.Application.DTOs;

public sealed record CustomerListItemDto(
    Guid Id,
    string FullNameAr,
    string FullNameEn,
    string MobileNumber,
    int LoyaltyPoints,
    decimal TotalSpend,
    bool IsActive);

public sealed class UpsertCustomerDto
{
    public Guid? Id { get; set; }
    public Guid BranchId { get; set; }
    public string FullNameAr { get; set; } = string.Empty;
    public string FullNameEn { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string? WhatsAppNumber { get; set; }
    public string? Email { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Notes { get; set; }
}
