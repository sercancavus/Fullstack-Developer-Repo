using Microsoft.EntityFrameworkCore;
using RestaurantApp.Common.Models;
using RestaurantApp.Data;
using RestaurantApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly ApplicationDbContext _context;

        public SaleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Sale>> GetDailySalesAsync()
        {
            return await _context.Sales
                .Where(s => s.SaleDate.Date == DateTime.Today)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .ToListAsync();
        }
        public async Task<IEnumerable<Sale>> GetWeeklySalesAsync()
        {
            var startOfWeek = DateTime.Today.AddDays(-((int)DateTime.Today.DayOfWeek));

            return await _context.Sales
                .Where(s => s.SaleDate.Date >= startOfWeek && s.SaleDate <= DateTime.Today)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .ToListAsync();
        }
        public async Task<IEnumerable<Sale>> GetMonthlySalesAsync()
        {
            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            return await _context.Sales
               .Where(s => s.SaleDate.Date >= startOfMonth && s.SaleDate <= DateTime.Today)
               .Include(s => s.SaleProducts)
               .ThenInclude(sp => sp.Product)
               .ToListAsync();
        }
        public async Task AddSalesAsync(Sale sale)
        {
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }
        public async Task AddSaleProductAsync(SaleProducts saleProducts)
        {
            await _context.SalesProducts.AddAsync(saleProducts);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime date)
        {
            return await _context.Sales
                .Where(s => s.SaleDate.Date == date)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .ToListAsync();
        }
        public async Task<IEnumerable<Sale>> GetSalesByDateAsync(DateTime date, string userId)
        {
            return await _context.Sales
                .Where(s => s.SaleDate.Date == date && s.SaleProducts.Any(sp => sp.UserId == userId))
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .ToListAsync();
        }
        public async Task<Sale> GetSaleByIdAsync(int saleId)
        {
            return await _context.Sales
                .Include(s => s.SaleProducts)
                .ThenInclude (sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);
        }
        public async Task UpdateSaleAsync(Sale sale)
        {
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }
    }
}
