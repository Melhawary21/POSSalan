using Microsoft.AspNetCore.Identity;

namespace SalonERP.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string DisplayNameAr { get; set; } = string.Empty;
    public string DisplayNameEn { get; set; } = string.Empty;
    public Guid? DefaultBranchId { get; set; }
    public Guid? CompanyId { get; set; }
    public bool IsActive { get; set; } = true;
}
