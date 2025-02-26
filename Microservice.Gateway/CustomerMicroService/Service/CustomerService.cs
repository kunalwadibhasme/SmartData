using CustomerMicroService.Database;
using CustomerMicroService.Dtos;
using CustomerMicroService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerMicroService.Service
{
    public class CustomerService
    {
        public readonly AppDbContext _appDbContext;

        public CustomerService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<object> AddCustomer(AddCustomer addCustomer)
        {
            var customer = await _appDbContext.customers.Where(c => c.Email == addCustomer.Email).FirstOrDefaultAsync();
            if (customer == null)
            {
                var customer1 = new Customer
                {
                    Name = addCustomer.Name,
                    Email = addCustomer.Email,
                    City = addCustomer.City,
                    Contact = addCustomer.Contact
                };
                await _appDbContext.customers.AddAsync(customer1);
                await _appDbContext.SaveChangesAsync();
                return new { status = 200, message = "Customer Added successfully" };
            }
            else
            {
                return new { status = 400, message = "Customer Already Exits" };
            }
        }

        public async Task<object> UpdateCustomer(int id, AddCustomer addCustomer)
        {
            var customer = await _appDbContext.customers.Where(u => u.Id == id).FirstOrDefaultAsync();
            if (customer == null)
            {
                return new { status = 400, message = "Customer with this id Not Found" };
            }
            else
            {
                customer.Contact = addCustomer.Contact;
                customer.Name = addCustomer.Name;
                customer.Email = addCustomer.Email;
                customer.City = addCustomer.City;
                _appDbContext.customers.Update(customer);
                await _appDbContext.SaveChangesAsync();
                return new { status = 200, message = "Customer Updated Successfully" };
            }
        }

        public async Task<object> GetAllCustomer()
        {
            var customer = await _appDbContext.customers.ToListAsync();
            return customer;
        }
    }
}
