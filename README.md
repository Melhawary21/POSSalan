# SalonERP

Enterprise-ready ASP.NET Core MVC 8 starter for a bilingual (Arabic/English) salon ERP-lite platform.

## Included foundation
- Layered architecture (Web/Application/Domain/Infrastructure)
- ASP.NET Core Identity setup with role seed and default admin
- SQL Server EF Core DbContext with Fluent configurations
- Core entities for branch, customer, service, appointment, invoice, payment, and cash shift
- Application services for customer management, booking conflict handling, and POS invoice creation
- MVC controllers + Razor views starter
- Localization plumbing and RTL/LTR layout awareness

## Default admin
- Email: `admin@salonerp.local`
- Password: `Admin@12345`

## Next implementation chunks
- Full permissions UI and menu policy engine
- Master data screens (services/products/suppliers/employees)
- Advanced appointment calendar and shift management dashboards
- Full report exporters and print templates
