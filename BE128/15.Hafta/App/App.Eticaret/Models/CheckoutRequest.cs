namespace App.Eticaret.Models
{
    internal class CheckoutRequest
    {
        public int UserId { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}