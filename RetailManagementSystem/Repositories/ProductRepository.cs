using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<Product> products, int TotalCount)> GetPagedProductsAsync(int pageNumber, int pageSize, string? filter)
        {
            var products = _context.Products
                  .Select(p => new Product
                  {
                      Id = p.Id,
                      ProductName = p.ProductName,
                      Description = p.Description,
                      Category = p.Category,
                      Price = p.Price,
                  });
            if (!string.IsNullOrEmpty(filter))
            {
                products= products.Where(p =>
                p.ProductName.Contains(filter) ||
                p.Id.ToString().Contains(filter) 
                );
            }

            var totalCount =await products.CountAsync();
            var pagedProducts =await products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedProducts, totalCount);
        }

        public async Task<List<int>> GetProductIdsAsync()
        {
           return await _context.Products
                .Select(p => p.Id)
                .ToListAsync();
        }

        public async Task<string?> GetProductNameByIdAsync(int productId)
        {
            var product= await _context.Products
                .FindAsync(productId);
            return product?.ProductName;
        }

        public async Task<decimal> GetProductPriceByIdAsync(int productId)
        {
            var product=await _context.Products
                .FindAsync(productId);
            return product?.Price ?? 0;
        }

        public async Task<int> GetProductsCountAsync()
        {
           return await _context.Products.CountAsync();
        }

        public Task<List<TopCategoryDto>> GetTopCategoriesAsync(int count)
        {
           var topCategories= _context.Products
                .GroupBy(p=> p.Category)
                .Select(g=> new TopCategoryDto
                {
                    Category= g.Key,
                    TotalProducts= g.Count()
                })
                .OrderByDescending(x=> x.TotalProducts)
                .Take(count)
                .ToListAsync();
            return topCategories;
        }
    }


}
