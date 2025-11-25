namespace App.Eticaret.Models.ViewModels
{
    public class ImageUploadViewModel
    {
        public bool IsRequired { get; set; }
        public bool IsMultipleFiles { get; set; }
        public string Name { get; set; } = null!;
        public string? Label { get; set; }
        public string? Accept { get; set; }
    }
}