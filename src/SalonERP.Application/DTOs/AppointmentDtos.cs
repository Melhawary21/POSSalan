namespace SalonERP.Application.DTOs;

public sealed class CreateAppointmentDto
{
    public Guid BranchId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? SpecialistEmployeeId { get; set; }
    public DateTime StartAtUtc { get; set; }
    public List<CreateAppointmentItemDto> Items { get; set; } = new();
    public string Source { get; set; } = "Reception";
    public string? Notes { get; set; }
}

public sealed class CreateAppointmentItemDto
{
    public Guid ServiceId { get; set; }
    public Guid? SpecialistEmployeeId { get; set; }
    public int DurationMinutes { get; set; }
    public decimal UnitPrice { get; set; }
}
