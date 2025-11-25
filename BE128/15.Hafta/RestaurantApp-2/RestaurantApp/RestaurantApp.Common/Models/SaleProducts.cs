using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Common.Models
{
    public class SaleProducts
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal ProductTotalPrice { get; set; }

        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public Sale Sale { get; set; }
        public Product Product { get; set; }
        public ApplicationUser User { get; set; }

    }
}
