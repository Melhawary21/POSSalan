using SalonERP.Application.DTOs;

namespace SalonERP.Application.Interfaces;

public interface IPosService
{
    Task<Guid> CreateInvoiceAsync(CreateInvoiceDto dto, CancellationToken cancellationToken = default);
}
