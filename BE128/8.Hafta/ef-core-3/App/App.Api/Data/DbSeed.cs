using App.Api.Data.Entities;

namespace App.Api.Data
{
    public static class DbSeed
    {
        public static async Task SeedAsync(AppDbContext dbContext)
        {
            var customer = new CustomerEntity
            {
                Name = "Ali Yılmaz",
                Email = "ali@gmail.com",
                Phone = "123456789"
            };

            dbContext.Customers.Add(customer);      

            await dbContext.SaveChangesAsync();

            var order1 = new OrderEntity
            {
                CustomerId = customer.Id,
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyy-MM-dd}-{customer.Id}"
            };

            dbContext.Orders.Add(order1);

            var order2 = new OrderEntity
            {
                CustomerId = customer.Id,
                OrderNumber = $"ORD-{DateTime.UtcNow:yyyy-MM-dd}-{customer.Id}"
            };


            dbContext.Orders.Add(order2);

            await dbContext.SaveChangesAsync();
        }
    }
}
