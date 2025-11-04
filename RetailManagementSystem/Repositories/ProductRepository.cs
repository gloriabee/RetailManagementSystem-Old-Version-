using RetailManagementSystem.DTOs;
using RetailManagementSystem.Interfaces;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(RetailDbContext context) : base(context)
        {

        }

        public Task<(List<Product> products, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, string? filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetProductIdsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetProductNameByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetProductPriceByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetProductsCountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TopCategoryDto>> GetTopCategoriesAsync(int count)
        {
            throw new NotImplementedException();
        }
    }


}
