namespace SalonERP.Domain.Enums;

public enum GenderApplicability
{
    WomenOnly = 1,
    MenOnly = 2,
    Both = 3
}

public enum AppointmentStatus
{
    Draft = 1,
    Confirmed = 2,
    CheckedIn = 3,
    InService = 4,
    Completed = 5,
    Cancelled = 6,
    NoShow = 7,
    Rescheduled = 8
}

public enum InvoiceStatus
{
    Draft = 1,
    Issued = 2,
    PartiallyPaid = 3,
    Paid = 4,
    Cancelled = 5,
    Refunded = 6
}

public enum InvoiceItemType
{
    Service = 1,
    Product = 2,
    Package = 3
}

public enum StockTransactionType
{
    GoodsReceipt = 1,
    StockIssue = 2,
    StockTransfer = 3,
    StockAdjustment = 4,
    StockCount = 5,
    Waste = 6
}
