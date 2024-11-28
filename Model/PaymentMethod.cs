namespace ZeraAPI.Model;

public class PaymentMethod
{
    public int PaymentMethodId { get; set; }
    public required string Method { get; set; }
    public required string CardNumber { get; set; }
    public required string ExpiryDate { get; set; }
    public required string CVV { get; set; }
    public required string NameOnCard { get; set; }
    public required string PayPalEmail { get; set; }
}