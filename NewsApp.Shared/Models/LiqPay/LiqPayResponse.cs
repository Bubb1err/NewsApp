using System.Text.Json.Serialization;

namespace NewsApp.Shared.Models;

public class LiqPayResponse
{
    [JsonPropertyName("checkout_url")]
    public string CheckoutUrl { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
}