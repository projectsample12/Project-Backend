using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace RetailOrdering.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) => _config = config;

        public async Task SendOrderConfirmationAsync(string toEmail, string orderNumber, decimal total)
        {
            var cfg = _config.GetSection("EmailSettings");
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MailboxAddress(cfg["FromName"], cfg["Username"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Order Confirmed - {orderNumber}";
            message.Body = new TextPart("html")
            {
                Text = $"<h2>Your order {orderNumber} is confirmed!</h2><p>Total: Rs. {total:F2}</p><p>Thank you for ordering!</p>"
            };
            using var client = new SmtpClient();
            await client.ConnectAsync(cfg["Host"], int.Parse(cfg["Port"]!), false);
            await client.AuthenticateAsync(cfg["Username"], cfg["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
