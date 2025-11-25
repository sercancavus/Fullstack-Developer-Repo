using RestaurantApp.Common.Models;

namespace RestaurantApp.Web.Models
{
    public class ReportViewModel
    {
        public string ReportTitle { get; set; }
        public DateTime ReportDate { get; set; }
        public List<Sale> Sales { get; set; }
    }
}
