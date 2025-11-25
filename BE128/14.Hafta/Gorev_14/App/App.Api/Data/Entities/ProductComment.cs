namespace App.Api.Data.Entities
{
    public class ProductComment
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } = null!;
        public string Comment { get; set; } = string.Empty;
        public int Stars { get; set; }
        public bool IsApproved { get; set; }
    }
}