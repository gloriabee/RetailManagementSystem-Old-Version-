using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RetailManagementSystem.Models;

namespace RetailManagementSystem;

public partial class RetailDbContext : DbContext
{

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {

            var config = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("E:\\Internship\\WPF\\RetailManagementSystem\\RetailManagementSystem\\appsettings.json", optional: false, reloadOnChange: true)
                            .Build();

            var connectionString = config.GetConnectionString("RetailDB");
            optionsBuilder.UseSqlServer(connectionString);


        }

    }
}

 