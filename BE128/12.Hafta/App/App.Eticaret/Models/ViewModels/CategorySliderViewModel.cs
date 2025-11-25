namespace App.Eticaret.Models.ViewModels
{
    public class CategorySliderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string IconCssClass { get; set; } = null!;
        public string? ImageUrl { get; set; }

    }
}
