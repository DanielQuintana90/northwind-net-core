using System;
using System.Collections.Generic;
using GenFu;
using Northwind.Api.Models;
using Northwind.Api.Repository.MySql;

namespace Northwind.Api.Integration.Tests.Builders
{
    public class CustomerBuilder
    {
        private readonly NorthwindDbContext _context;

        public CustomerBuilder(NorthwindDbContext context)
        {
            _context = context;
            CleanCustomerTable();
        }

        public CustomerBuilder WithSpecificCustomer(Customer customer)
        {
            AddCustomerToDbContext(customer);
            return this;
        }

        public CustomerBuilder WithCustomerAndIdValue(int id)
        {
            A.Configure<Customer>()
                .Fill(c => c.Id, () => id);

            AddCustomerToDbContext(A.New<Customer>());           
            return this;    
        }

        public CustomerBuilder With10Customers()
        {
            AddCustomersToDbContext(CreateCustomers(10));
            return this;
        }

        public NorthwindDbContext Build()
        {
            return _context;
        }

        private void AddCustomersToDbContext(IEnumerable<Customer> customers)
        {
            _context.AddRange(customers);
            _context.SaveChanges();    
        }

        private void AddCustomerToDbContext(Customer customer)
        {
            _context.Add(customer);
            _context.SaveChanges();
        }

        private IEnumerable<Customer> CreateCustomers(int quantity)
        {
            int id = 1;

            GenFu.GenFu.Configure<Customer>()
                .Fill(c => c.Id, () => id++);

            return A.ListOf<Customer>(quantity);
        }

        public void CleanCustomerTable()
        {
            _context.RemoveRange(_context.Customers);
            _context.SaveChanges();
        }
    }
}
