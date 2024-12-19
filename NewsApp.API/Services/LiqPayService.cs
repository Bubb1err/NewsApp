using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using NewsApp.Shared.Models;

namespace NewsApp.API.Services;

public class LiqPayService
{
    private readonly string _publicKey;
    private readonly  AccessControlService _accessControl;
    private readonly string _privateKey;
    private readonly HttpClient _httpClient;
    private readonly ILogger<LiqPayService> _logger;
    private const string API_URL = "https://www.liqpay.ua/api/";

    public LiqPayService(
        IConfiguration configuration,
        AccessControlService accessControl,
        HttpClient httpClient,
        ILogger<LiqPayService> logger)
    {
        _publicKey = configuration["LiqPay:PublicKey"];
        _privateKey = configuration["LiqPay:PrivateKey"];
        _httpClient = httpClient;
        _accessControl = accessControl;
        _logger = logger;
    }

    public async Task<string> CreateSubscriptionAsync(string email, string id, string redirectUrl)
    {
        var ngrokUrl = "https://62b4-91-202-130-54.ngrok-free.app"; // Замените на ваш URL от ngrok

        try
        {
            var subscriptionData = new Dictionary<string, string>
            {
                { "public_key", _publicKey },
                { "version", "3" },
                { "action", "subscribe" },
                { "amount", "5"},
                { "currency", "USD" },
                { "description", "NewsPremium" },
                { "order_id", Guid.NewGuid().ToString("N") },
                { "email", email },
                {"customer" , id},
                { "subscribe", "1" },
                { "subscribe_date_start", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                { "subscribe_periodicity", "month" },
                { "result_url", redirectUrl }, // URL для перенаправления после оплаты
                { "server_url", $"{ngrokUrl}/api/Subscription/callback" } // URL для получения уведомлений
            };

            var jsonString = JsonSerializer.Serialize(subscriptionData);
            var dataBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonString));
            var signature = GenerateSignature(dataBase64);

            var checkoutUrl = $"https://www.liqpay.ua/api/3/checkout?data={dataBase64}&signature={signature}";
            
            _logger.LogInformation($"Generated checkout URL: {checkoutUrl}");
            return checkoutUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription");
            throw;
        }
    }

    private string GenerateSignature(string data)
    {
        using var sha1 = SHA1.Create();
        var signature = sha1.ComputeHash(
            Encoding.UTF8.GetBytes(_privateKey + data + _privateKey));
        return Convert.ToBase64String(signature);
    }

    // Метод для проверки подписи callback'а
    public bool ValidateCallback(string data, string signature)
    {
        var expectedSignature = GenerateSignature(data);
        return signature == expectedSignature;
    }
}