using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<RetailDbContext>
    {
        public RetailDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();


            var connectionString = configuration.GetConnectionString("RetailDB");

            var optionsBuilder = new DbContextOptionsBuilder<RetailDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new RetailDbContext(optionsBuilder.Options);

        }
    }
}
