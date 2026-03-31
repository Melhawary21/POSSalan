using SalonERP.Application.DTOs;

namespace SalonERP.Application.Interfaces;

public interface ICustomerService
{
    Task<IReadOnlyList<CustomerListItemDto>> SearchAsync(Guid branchId, string? keyword, CancellationToken cancellationToken = default);
    Task<Guid> UpsertAsync(UpsertCustomerDto dto, string userId, CancellationToken cancellationToken = default);
}
