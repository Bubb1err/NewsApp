using System.Text.Json.Serialization;

namespace NewsApp.Shared.Models;

public class LiqPayCallback
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }

    [JsonPropertyName("amount")]
    public double Amount { get; set; }
    
    [JsonPropertyName("customer")]
    public string Customer { get; set; }

    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}