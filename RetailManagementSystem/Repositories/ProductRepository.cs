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
    public class ProductRepository : IProductRepository
    {
        public Task<List<Product>> GetPagedProductsAsync(string? filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetProductIdsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetProductNameByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetProductPriceByIdAsync(int productId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<TopCategoryDto>> GetTopCategoriesAsync(int count, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCountAsync(string? filter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
