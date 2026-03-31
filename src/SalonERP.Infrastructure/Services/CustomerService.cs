using Microsoft.EntityFrameworkCore;
using SalonERP.Application.DTOs;
using SalonERP.Application.Interfaces;
using SalonERP.Domain.Entities;
using SalonERP.Infrastructure.Data;

namespace SalonERP.Infrastructure.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _dbContext;

    public CustomerService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<CustomerListItemDto>> SearchAsync(Guid branchId, string? keyword, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Customers
            .AsNoTracking()
            .Where(x => x.BranchId == branchId);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.FullNameAr.Contains(keyword) || x.FullNameEn.Contains(keyword) || x.MobileNumber.Contains(keyword));
        }

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new CustomerListItemDto(x.Id, x.FullNameAr, x.FullNameEn, x.MobileNumber, x.LoyaltyPoints, x.TotalSpend, x.IsActive))
            .ToListAsync(cancellationToken);
    }

    public async Task<Guid> UpsertAsync(UpsertCustomerDto dto, string userId, CancellationToken cancellationToken = default)
    {
        var normalizedMobile = dto.MobileNumber.Trim();

        if (dto.Id is null)
        {
            var duplicate = await _dbContext.Customers.AnyAsync(x => x.BranchId == dto.BranchId && x.MobileNumber == normalizedMobile, cancellationToken);
            if (duplicate)
            {
                throw new InvalidOperationException("Customer mobile already exists in this branch.");
            }

            var entity = new Customer
            {
                BranchId = dto.BranchId,
                FullNameAr = dto.FullNameAr.Trim(),
                FullNameEn = dto.FullNameEn.Trim(),
                MobileNumber = normalizedMobile,
                WhatsAppNumber = dto.WhatsAppNumber?.Trim(),
                Email = dto.Email?.Trim(),
                DateOfBirth = dto.DateOfBirth,
                Notes = dto.Notes,
                CreatedByUserId = userId
            };

            _dbContext.Customers.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }

        var customer = await _dbContext.Customers.FirstAsync(x => x.Id == dto.Id.Value, cancellationToken);
        customer.FullNameAr = dto.FullNameAr.Trim();
        customer.FullNameEn = dto.FullNameEn.Trim();
        customer.MobileNumber = normalizedMobile;
        customer.WhatsAppNumber = dto.WhatsAppNumber?.Trim();
        customer.Email = dto.Email?.Trim();
        customer.DateOfBirth = dto.DateOfBirth;
        customer.Notes = dto.Notes;
        customer.UpdatedAtUtc = DateTime.UtcNow;
        customer.UpdatedByUserId = userId;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }
}
