namespace RestaurantApp.Web.Models
{
    public class EditOrderModel
    {
        public int OrderId { get; set; }
        public List<EditOrderProductModel> Products { get; set; }
    }
}
