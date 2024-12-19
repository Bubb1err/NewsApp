namespace NewsApp.Shared.Models;

public class SubscriptionRequest
{
    public string Email { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
}