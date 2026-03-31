namespace SalonERP.Application.Services.Common;

public sealed class BusinessException : Exception
{
    public BusinessException(string message) : base(message)
    {
    }
}
