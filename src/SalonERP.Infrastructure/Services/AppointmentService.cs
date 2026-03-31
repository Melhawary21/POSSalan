using Microsoft.EntityFrameworkCore;
using SalonERP.Application.DTOs;
using SalonERP.Application.Interfaces;
using SalonERP.Application.Services.Common;
using SalonERP.Domain.Entities;
using SalonERP.Infrastructure.Data;

namespace SalonERP.Infrastructure.Services;

public sealed class AppointmentService : IAppointmentService
{
    private readonly ApplicationDbContext _dbContext;

    public AppointmentService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateAsync(CreateAppointmentDto dto, string userId, CancellationToken cancellationToken = default)
    {
        if (dto.Items.Count == 0)
        {
            throw new BusinessException("At least one service is required.");
        }

        var totalMinutes = dto.Items.Sum(x => x.DurationMinutes);
        var endAtUtc = dto.StartAtUtc.AddMinutes(totalMinutes);

        var hasConflict = await _dbContext.Appointments
            .AsNoTracking()
            .AnyAsync(x => x.BranchId == dto.BranchId
                           && x.SpecialistEmployeeId == dto.SpecialistEmployeeId
                           && x.StartAtUtc < endAtUtc
                           && x.EndAtUtc > dto.StartAtUtc,
                cancellationToken);

        if (hasConflict)
        {
            throw new BusinessException("Specialist already has an overlapping appointment.");
        }

        var appointment = new Appointment
        {
            BranchId = dto.BranchId,
            CustomerId = dto.CustomerId,
            SpecialistEmployeeId = dto.SpecialistEmployeeId,
            StartAtUtc = dto.StartAtUtc,
            EndAtUtc = endAtUtc,
            Source = dto.Source,
            Notes = dto.Notes,
            CreatedByUserId = userId,
            Items = dto.Items.Select(x => new AppointmentItem
            {
                ServiceId = x.ServiceId,
                SpecialistEmployeeId = x.SpecialistEmployeeId,
                DurationMinutes = x.DurationMinutes,
                UnitPrice = x.UnitPrice,
                CreatedByUserId = userId
            }).ToList()
        };

        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return appointment.Id;
    }
}
