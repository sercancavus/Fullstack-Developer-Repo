namespace App.Eticaret.Models.ViewModels
{
    public class ProductListingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public decimal Price { get; set; }
        public byte? DiscountPercentage { get; set; }

        public decimal? DiscountedPrice =>
            DiscountPercentage.HasValue ? Price - (Price * DiscountPercentage.Value / 100) : null;

        public string? ImageUrl { get; set; }
    }
}
