namespace App.Eticaret.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public string OrderCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = [];
    }

    public class OrderItemViewModel
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
