﻿using Flogging.Data.Entity;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace FloggingConsole.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalPurchases { get; set; }
        public decimal TotalReturns { get; set; }
    }

    public class CustomerDbContext: DbContext
    {
        public CustomerDbContext()
        {
            DbInterception.Add(new FloggerEFInterceptor());
        }
        public DbSet<Customer> Customers { get; set; }
    }
}
