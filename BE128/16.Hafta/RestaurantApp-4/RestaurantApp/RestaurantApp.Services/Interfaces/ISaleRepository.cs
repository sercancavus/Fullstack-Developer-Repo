using RestaurantApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Interfaces
{
    public interface ISaleRepository
    {
        Task<IEnumerable<Sale>> GetDailySalesAsync();
        Task<IEnumerable<Sale>> GetWeeklySalesAsync();
        Task<IEnumerable<Sale>> GetMonthlySalesAsync();
        Task AddSaleAsync(Sale sale);
        Task AddSaleProductAsync(SaleProducts saleProducts);
        Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime date);
        Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime date, string userId);
        Task<Sale> GetSaleByIdAsync(int saleId);
        Task UpdateSaleAsync(Sale sale);

    }
}
