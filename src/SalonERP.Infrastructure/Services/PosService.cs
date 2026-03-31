using Microsoft.EntityFrameworkCore;
using SalonERP.Application.DTOs;
using SalonERP.Application.Interfaces;
using SalonERP.Domain.Entities;
using SalonERP.Domain.Enums;
using SalonERP.Infrastructure.Data;

namespace SalonERP.Infrastructure.Services;

public sealed class PosService : IPosService
{
    private readonly ApplicationDbContext _dbContext;

    public PosService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> CreateInvoiceAsync(CreateInvoiceDto dto, CancellationToken cancellationToken = default)
    {
        var subTotal = dto.Items.Sum(x => x.Quantity * x.UnitPrice);
        var net = subTotal - dto.DiscountAmount + dto.TaxAmount;
        var paid = dto.Payments.Sum(x => x.Amount);
        var due = net - paid;

        var invoice = new Invoice
        {
            BranchId = dto.BranchId,
            CustomerId = dto.CustomerId,
            AppointmentId = dto.AppointmentId,
            CashierUserId = dto.CashierUserId,
            InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}",
            SubTotal = subTotal,
            DiscountTotal = dto.DiscountAmount,
            TaxTotal = dto.TaxAmount,
            NetTotal = net,
            PaidAmount = paid,
            DueAmount = due,
            Status = due <= 0 ? InvoiceStatus.Paid : InvoiceStatus.PartiallyPaid,
            Items = dto.Items.Select(x => new InvoiceItem
            {
                ItemType = (InvoiceItemType)x.ItemType,
                ReferenceId = x.ReferenceId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                NetAmount = x.Quantity * x.UnitPrice
            }).ToList()
        };

        await using var trx = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        _dbContext.Invoices.Add(invoice);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var payment in dto.Payments)
        {
            _dbContext.Payments.Add(new Payment
            {
                BranchId = dto.BranchId,
                InvoiceId = invoice.Id,
                MethodCode = payment.MethodCode,
                Amount = payment.Amount,
                PaidAtUtc = DateTime.UtcNow,
                ReferenceNo = payment.ReferenceNo
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        await trx.CommitAsync(cancellationToken);

        return invoice.Id;
    }
}
