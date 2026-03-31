using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalonERP.Application.DTOs;
using SalonERP.Application.Interfaces;

namespace SalonERP.Web.Controllers;

[Authorize]
public sealed class CustomersController : Controller
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    public async Task<IActionResult> Index(Guid branchId, string? keyword, CancellationToken cancellationToken)
    {
        var items = await _customerService.SearchAsync(branchId, keyword, cancellationToken);
        ViewData["BranchId"] = branchId;
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(UpsertCustomerDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var id = await _customerService.UpsertAsync(dto, User.Identity?.Name ?? "system", cancellationToken);
        return RedirectToAction(nameof(Index), new { branchId = dto.BranchId, id });
    }
}
