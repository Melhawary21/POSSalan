# Salon ERP-Lite (Gulf Market) — Business Analysis & Solution Blueprint

## 1) Full Business Analysis

### 1.1 Business Context and Goals
This solution targets **single-company, multi-branch** salon operations with immediate support for both:
- Women salons (hair, makeup, nails, spa, bridal, skin)
- Men/barber salons (haircut, beard, grooming, facial, massage)

Primary business goals:
1. Centralize operations across branches.
2. Increase booking utilization and reduce no-shows.
3. Improve cashier control and daily closing accuracy.
4. Track specialist productivity and commissions accurately.
5. Build customer retention through loyalty, memberships, and targeted offers.
6. Ensure bilingual usability (Arabic/English) and Gulf-market compliance style.

### 1.2 Operating Models Supported
- **Appointment-first model** (scheduled operations).
- **Walk-in-first model** (barber-style throughput operations).
- **Hybrid model** (both scheduled + walk-in).
- **Service + Retail** (services and product sales in one invoice).
- **Single specialist or multi-specialist booking** in one customer visit.

### 1.3 Key Personas and Outcomes
- **Owner/Super Admin**: full visibility, branch performance, profit and tax reporting.
- **Branch Manager**: branch-level operations, staffing, shift oversight.
- **Receptionist**: appointment booking, customer profile upkeep, check-in.
- **Cashier**: POS, split payment, refunds, shift open/close.
- **Specialist/Barber/Technician**: personal schedule, service execution, commission tracking.
- **Inventory Officer**: stock receipts/transfers/adjustments and low stock control.
- **Accountant**: invoice/payment reconciliation, expenses, tax summaries.
- **HR**: attendance, leaves, employee lifecycle.

### 1.4 Core Domain Flows
1. **Customer lifecycle**: lead/walk-in → profile enrichment → appointments/invoices → loyalty/membership retention.
2. **Booking lifecycle**: create → confirm → check-in → in-service → completed/no-show/cancelled/rescheduled.
3. **POS lifecycle**: cart build (services/products) → discount/coupon/membership → tax → payment(s) → invoice issue.
4. **Cash shift lifecycle**: open shift → transactions/cash movements → reconciliation → close → manager approval.
5. **Inventory lifecycle**: purchase receipt → branch stock → sale/consumption → adjustments/waste → reorder alert.
6. **Commission lifecycle**: rules assignment → qualifying events (completed service/paid invoice) → accrual → payout reporting.

### 1.5 Non-Functional Requirements (Enterprise)
- Security: strong auth, policy/permission enforcement, audit logging.
- Performance: indexed queries, projection-based reads, pagination.
- Reliability: transactional boundaries on financial flows.
- Maintainability: layered architecture, isolated business services.
- Extensibility: future API/mobile and SaaS multi-company readiness.
- Localization: full Arabic/English resources with RTL/LTR layouts.

---

## 2) Final Architecture Decision

### 2.1 Architecture Style
**Layered Modular Monolith** with clear boundaries:
- **Presentation Layer**: ASP.NET Core MVC (Areas, Controllers, Views, ViewComponents)
- **Application Layer**: use-case orchestration, DTOs, service interfaces, validators
- **Domain Layer**: entities, enums, business rules, domain services/policies
- **Infrastructure Layer**: EF Core, Identity, file storage, notifications, reporting adapters

This is chosen over microservices for faster delivery, lower ops complexity, and sufficient scale for ERP-lite multi-branch.

### 2.2 Boundaries and Responsibilities
- Controllers: request/response, model binding, authorization, no heavy business logic.
- Application Services: transactional business workflows, validation orchestration, logging hooks.
- DbContext + Fluent Configurations: persistence and constraints.
- Domain entities: rule-safe state transitions (e.g., Appointment status progression).
- Cross-cutting services: localization, current user/branch context, audit, caching.

### 2.3 Future SaaS-Ready Approach
All transactional aggregates include optional **CompanyId** and required **BranchId** where applicable; query filters and tenant strategy are abstracted behind context providers to enable future tenant isolation.

---

## 3) Database Design (SQL Server)

### 3.1 Base Conventions
Most business tables include:
- `Id (uniqueidentifier)` PK
- `CompanyId (uniqueidentifier)` nullable now, non-null for SaaS later
- `BranchId (uniqueidentifier)` nullable for global tables, required for branch-scoped tables
- `CreatedAtUtc`, `CreatedByUserId`
- `UpdatedAtUtc`, `UpdatedByUserId`
- `IsActive` bit default 1
- `IsDeleted` bit default 0
- `RowVersion` rowversion for optimistic concurrency (critical tables)

### 3.2 Identity & Authorization
1. **AspNetUsers** (extended as ApplicationUser)
2. **AspNetRoles**
3. **AspNetUserRoles**
4. **Permissions** (`Code`, `Module`, `Action`, `Description`)
5. **RolePermissions** (RoleId + PermissionId)
6. **UserBranchAccesses** (user to branches)

### 3.3 Core Setup Tables
- `Companies`
- `Branches`
- `SystemSettings`
- `BusinessHours` (per branch/day)
- `TaxCodes`
- `Currencies`
- `PaymentMethods`
- `ShiftTypes`
- `InvoiceStatuses`
- `AppointmentStatuses`

### 3.4 Master & Operations
- `Customers`
- `CustomerTags`, `CustomerTagMappings`
- `CustomerNotes`
- `Employees`
- `Specialists` (1:1 with Employee)
- `SpecialistServices`
- `EmployeeSchedules`
- `Attendances`
- `LeaveRequests`
- `ServiceCategories`
- `Services`
- `ServiceAddOns`
- `Packages`
- `PackageItems`
- `Suppliers`
- `UnitsOfMeasure`
- `ProductCategories`
- `Products`
- `BranchStocks`
- `StockTransactions`
- `StockTransactionItems`
- `StockBatches` (optional expiry/batch control)

### 3.5 Booking & Service Execution
- `Appointments`
- `AppointmentItems` (service lines)
- `AppointmentItemAssignments` (specialist/chair per line)
- `ServiceStations` (chairs/rooms)
- `AppointmentStatusLogs`

### 3.6 POS, Invoicing, Payments
- `Invoices`
- `InvoiceItems` (service/product/package)
- `InvoiceDiscounts`
- `Payments`
- `PaymentAllocations`
- `Refunds`
- `CreditNotes`
- `SuspendedSales`

### 3.7 Cash Shift & Expenses
- `CashShifts`
- `CashMovements` (cash in/out)
- `ShiftReconciliations`
- `ExpenseCategories`
- `Expenses`

### 3.8 Loyalty, Membership, Promotion
- `LoyaltySettings`
- `LoyaltyTransactions`
- `MembershipPlans`
- `MembershipPlanServices`
- `MembershipSubscriptions`
- `MembershipUsages`
- `Coupons`
- `CouponRedemptions`
- `GiftVouchers`

### 3.9 Reporting, Notifications, Audit
- `Notifications`
- `NotificationRecipients`
- `AuditLogs`
- `Attachments`
- `ReportSnapshots` (optional pre-aggregated datasets)

### 3.10 Critical Relationships (selected)
- Branches → Customers/Employees/Appointments/Invoices/CashShifts/Expenses (1:M)
- Customers → Appointments/Invoices/LoyaltyTransactions/Memberships (1:M)
- Services → AppointmentItems/InvoiceItems/PackageItems (1:M)
- Invoices → InvoiceItems/Payments/Refunds (1:M)
- Appointments → AppointmentItems (1:M) and optional Invoice (1:1)
- Employees ↔ Services via SpecialistServices (M:M)

### 3.11 Constraint & Index Strategy (selected)
- Unique: `Branches(Code)`, `Services(Code, BranchId)` when branch-scoped, `Products(Code, BranchId)`.
- Unique filtered: one active open cash shift per cashier per branch.
- Nonclustered indexes:
  - `Appointments(BranchId, StartTimeUtc, Status)`
  - `Invoices(BranchId, InvoiceDateUtc, Status)`
  - `Payments(InvoiceId, PaymentDateUtc)`
  - `BranchStocks(BranchId, ProductId)` unique
  - `Customers(MobileNumber)` include `IsDeleted`
- Soft delete with global query filters for operational tables.

---

## 4) ASP.NET Core MVC 8 Folder Structure

```text
src/
  SalonERP.Web/
    Areas/
      Admin/
        Controllers/
        Views/
    Controllers/
    Views/
      Shared/
    ViewModels/
    wwwroot/
      themes/dreamspos/
      css/
      js/
      lib/
    Resources/
      SharedResource.en.resx
      SharedResource.ar.resx
  SalonERP.Application/
    Interfaces/
    Services/
    DTOs/
    Validators/
    Mapping/
    Contracts/
  SalonERP.Domain/
    Entities/
    Enums/
    ValueObjects/
    Policies/
    Events/
    Common/
  SalonERP.Infrastructure/
    Data/
      Configurations/
      Seed/
      Migrations/
    Identity/
    Repositories/ (only if needed)
    Localization/
    Reporting/
    Notifications/
  SalonERP.Shared/
    Constants/
    Exceptions/
    Helpers/
tests/
  SalonERP.UnitTests/
  SalonERP.IntegrationTests/
```

---

## 5) Module Dependency Map

1. **Foundation**: Identity, Permissions, Localization, Company/Branch context.
2. **Master Data**: service/product/customer/employee taxonomies.
3. **Scheduling**: appointment engine depends on Customers + Specialists + Services + Stations.
4. **Sales/POS**: depends on Appointments (optional), Services, Products, Tax, Coupons, Membership.
5. **Payments/Shift**: depends on Invoices and PaymentMethods.
6. **Inventory**: linked to Products and POS consumption/sales.
7. **Loyalty/Membership**: depends on Invoices and Customers.
8. **Commissions**: depends on completed services and collected invoices.
9. **Reports/Dashboards**: read models over all transactional modules.

---

## 6) Security and Permission Model

### 6.1 Authentication
- ASP.NET Core Identity with cookie auth.
- Password policy, lockout, reset token, security stamp validation.
- Optional 2FA extensibility point.

### 6.2 Authorization
- Role + permission hybrid:
  - Roles grant baseline access.
  - Permissions enforce page/action/menu granular control.
- Policy naming convention: `Permission.{Module}.{Action}`.
- Custom authorization handler validates current user claims/role-permission cache.

### 6.3 Data Access Security
- Branch scoping middleware injects accessible branches context.
- Query-level branch predicates for branch-scoped records.
- Super Admin bypass policy for global access.

### 6.4 Auditability
- SaveChanges interceptor captures CRUD diffs for critical tables.
- Immutable audit trail with actor/user, IP, branch, timestamp, action.

---

## 7) Localization and RTL/LTR Strategy

1. Resource files for all UI text/validation/statuses.
2. `IStringLocalizer<SharedResource>` for controllers/views/services.
3. Culture middleware with cookie (`c=en-US` / `c=ar-KW`) persistence.
4. `RequestLocalizationOptions` with supported cultures `[en-US, ar-KW]`.
5. Layout sets:
   - `dir="rtl"` + Arabic font stack for Arabic.
   - `dir="ltr"` for English.
6. Conditional loading:
   - `dreamspos.css` + `dreamspos-rtl.css`.
7. Date/currency formatting by selected culture.

---

## 8) DreamsPOS-v2.2.5 Theme Integration Strategy

1. Keep vendor assets under `wwwroot/themes/dreamspos/` untouched.
2. Build `site-enterprise.css` as override layer (tokens, spacing, validation, cards).
3. Create reusable partials:
   - `_SidebarMenu.cshtml`
   - `_Topbar.cshtml`
   - `_Breadcrumb.cshtml`
   - `_FilterPanel.cshtml`
   - `_StatCard.cshtml`
4. Menu rendering from permission map (server-side).
5. Standard page template:
   - Header, breadcrumb, filter row, table/form card, actions.
6. Standard data table with server-side paging/filtering/sorting contract.

---

## 9) Implementation Roadmap (Execution Plan)

### Phase 0 — Foundation
- Create solution/projects and references.
- Configure SQL Server, Identity, localization, error handling pipeline.
- Implement base entities, soft delete, audit interceptor.

### Phase 1 — Security & Setup
- Authentication (login/forgot/reset/profile).
- Role/permission management UI + seed defaults.
- Company/branch/settings modules.

### Phase 2 — Master Data
- Customers, Employees/Specialists, Services/Categories, Products/Categories/UoM/Suppliers.

### Phase 3 — Operations Core
- Appointment engine with conflict detection and status transitions.
- POS + Invoicing + payments/split payments/refunds.
- Cash shift management with variance and approvals.

### Phase 4 — Advanced Commercial
- Inventory transactions and low stock alerts.
- Loyalty/Membership/Coupons/Gift vouchers.
- Commission engine and payout reports.

### Phase 5 — Analytics & UX Hardening
- Dashboards and reports with branch/date filters.
- Excel export, print views, notification center.
- UI consistency and accessibility pass.

### Phase 6 — Deployment Readiness
- Seed default admin and master data.
- Migration scripts, environment config, logging and health checks.
- UAT checklist and production hardening.

---

## Next Step (Code Generation Gate)
Per your instruction, this document completes the analysis stage first. Next step is to start **Step 4 base solution code generation** and then proceed module-by-module in the agreed order.
