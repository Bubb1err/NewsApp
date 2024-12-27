using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using NewsApp.Shared.Constants;
using NewsApp.Shared.Models;

namespace NewsApp.API.Services;

public class LiqPayService
{
    private readonly string _publicKey;
    private readonly  AccessControlService _accessControl;
    private readonly string _privateKey;
    private readonly HttpClient _httpClient;
    private readonly string ngrokUrl ; 
    private readonly string ngrokUrl2 ; 

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
        ngrokUrl = Urls.ServerUrl;
        ngrokUrl2 = Urls.UiUrl;
        _httpClient = httpClient;
        _accessControl = accessControl;
        _logger = logger;
    }

    public async Task<string> CreateSubscriptionAsync(string email, string id, string redirectUrl)
    {
       
        try
        {
            var subscriptionData = new Dictionary<string, string>
            {
                { "public_key", _publicKey },
                { "version", "3" },
                { "action", "subscribe" },
                { "amount", "200"},
                { "currency", "UAH" },
                { "description", "NewsPremium" },
                { "order_id", Guid.NewGuid().ToString("N") },
                { "email", email },
                {"customer" , id},
                { "subscribe", "1" },
                { "subscribe_date_start", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") },
                { "subscribe_periodicity", "month" },
                { "result_url", $"{ngrokUrl2}/news" }, 
                { "server_url", $"{ngrokUrl}/api/Subscription/callback" } 
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

    public bool ValidateCallback(string data, string signature)
    {
        var expectedSignature = GenerateSignature(data);
        return signature == expectedSignature;
    }
}