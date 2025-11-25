namespace App.Eticaret.Models.ViewModels
{
    public class HomeProductDetailViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public byte? DiscountRate { get; set; }

        public decimal? DiscountedPrice =>
            DiscountRate.HasValue ? Price - (Price * DiscountRate.Value / 100) : null;

        public string Description { get; set; } = null!;

        public byte StockAmount { get; set; }

        public string SellerName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public int CategoryId { get; set; }

        public string[] ImageUrls { get; set; } = [];

        public ProductReviewViewModel[] Reviews { get; set; } = [];

        public double? AverageStarCount =>
            Reviews.Length == 0 ? null : Reviews.Average(r => r.StarCount);

        public bool IsInCart { get; set; }
    }

    public class ProductReviewViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public byte StarCount { get; set; }
        public string UserName { get; set; } = null!;
    }
}
