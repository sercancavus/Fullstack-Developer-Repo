using App.Api.Data;
using App.Api.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Contracts;

namespace App.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var customers = await _dbContext.Customers
                .Include(x => x.Orders)
                .ToListAsync();

            // Include işlemi : dahil etme anlamına gelir (INNER JOIN)

            return Ok(customers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var customer = await _dbContext.Customers
                .Include(x => x.Orders)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerEntity customer)
        {
            _dbContext.Customers.Add(customer);

            await _dbContext.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] CustomerEntity customer)
        {
            var dbCustomer = await _dbContext.Customers.FindAsync(id);

            if (dbCustomer is null)
            {
                return BadRequest();
            }

            // güncelleme yapmak için kullanılır
            _dbContext.Entry(customer).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer is null)
            {
                return BadRequest();
            }

            _dbContext.Customers.Remove(customer);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
