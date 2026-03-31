using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalonERP.Domain.Entities;
using SalonERP.Infrastructure.Identity;

namespace SalonERP.Infrastructure.Data.Seed;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        await context.Database.MigrateAsync();

        var roles = new[]
        {
            "SuperAdmin", "Owner", "BranchManager", "Receptionist", "Cashier", "Specialist", "InventoryOfficer", "Accountant", "HR", "Viewer"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        if (!await context.Companies.AnyAsync())
        {
            var company = new Company { NameAr = "شركة الصالون", NameEn = "Salon Company" };
            var branch = new Branch { Code = "HQ", NameAr = "الفرع الرئيسي", NameEn = "Head Branch" };
            context.Companies.Add(company);
            context.Branches.Add(branch);
            await context.SaveChangesAsync();
        }

        const string adminEmail = "admin@salonerp.local";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                DisplayNameAr = "مدير النظام",
                DisplayNameEn = "System Administrator"
            };

            var createResult = await userManager.CreateAsync(admin, "Admin@12345");
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join(';', createResult.Errors.Select(x => x.Description)));
            }

            await userManager.AddToRoleAsync(admin, "SuperAdmin");
        }
    }
}
