using SalonERP.Application.DTOs;

namespace SalonERP.Application.Interfaces;

public interface IAppointmentService
{
    Task<Guid> CreateAsync(CreateAppointmentDto dto, string userId, CancellationToken cancellationToken = default);
}
