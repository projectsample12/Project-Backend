namespace RetailOrdering.API.Interfaces;

public interface IEmailService
{
    Task SendOrderConfirmationAsync(string toEmail, string orderNumber, decimal total);
}