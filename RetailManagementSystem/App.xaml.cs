using Microsoft.Extensions.DependencyInjection;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Repositories;
using System;
using System.Windows;

namespace RetailManagementSystem
{
    public partial class App : Application
    {
        public App()
        {
            IServiceCollection services = new ServiceCollection();

            // repositories 
            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();


            // unit of work 
            services.AddTransient<IUnitOfWork, UnitOfWork>();



        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Enable binding error logging
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
        }
    }
}