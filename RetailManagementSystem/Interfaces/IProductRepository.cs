using RetailManagementSystem.DTOs;
using RetailManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetailManagementSystem.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<int> GetProductsCountAsync();
        Task<List<int>> GetProductIdsAsync();
        Task<string?> GetProductNameByIdAsync(int productId);
        Task<decimal> GetProductPriceByIdAsync(int productId);
        Task<List<TopCategoryDto>> GetTopCategoriesAsync(int count);
        Task<(List<Product> products, int TotalCount)>
            GetPagedProductsAsync
            (int pageNumber, int pageSize, string? filter);
    }
}
