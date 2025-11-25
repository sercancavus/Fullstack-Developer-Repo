using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Common.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<SaleProducts> Orders { get; set; }
    }
}
