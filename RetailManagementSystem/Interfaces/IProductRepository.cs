using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetPagedProductsAsync(string? filter, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<int> GetTotalCountAsync(string? filter, CancellationToken cancellationToken = default);
        Task<List<int>> GetProductIdsAsync(CancellationToken cancellationToken = default);
        Task<string> GetProductNameByIdAsync(int productId, CancellationToken cancellationToken = default);
        Task<decimal> GetProductPriceByIdAsync(int productId, CancellationToken cancellationToken = default);
        Task<List<TopCategoryDto>> GetTopCategoriesAsync(int count, CancellationToken cancellationToken = default);
    }
}
