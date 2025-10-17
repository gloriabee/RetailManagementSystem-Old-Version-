using System;
using System.Collections.Generic;
using System.Linq;
using RetailManagementSystem.Models;

namespace RetailManagementSystem.Services
{
    public class ProductService
    {
        private readonly RetailDbContext _context;

        public ProductService()
        {
            _context = new RetailDbContext();
        }

        // Get paged products with optional filter (by name or category)
        public List<Product> GetPagedProducts(string filter, int pageNumber, int pageSize)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p =>
                    p.ProductName.ToLower().Contains(filter.ToLower()) ||
                    p.Category.ToLower().Contains(filter.ToLower()) ||
                    p.Id.ToString().Contains(filter)
                );
            }

            return query
                .OrderBy(p => p.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new Product
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Category = p.Category,
                    Price = p.Price,
                }).ToList();
        }

        // Get total count for paging
        public int GetTotalCount(string filter)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(p =>
                    p.ProductName.ToLower().Contains(filter.ToLower()) ||
                    p.Id.ToString().Contains(filter)
                );
            }

            return query.Count();
        }

        // Add a new product
        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        

        // Update product
        public void Update(Product product)
        {

            var productToUpdate = _context.Products.Find(product.Id);
            if (productToUpdate != null)
            {
                productToUpdate.ProductName = product.ProductName;
                productToUpdate.Description = product.Description;
                productToUpdate.Category = product.Category;
                productToUpdate.Price = product.Price;
                productToUpdate.UpdatedAt = DateTime.UtcNow;
                _context.SaveChanges();
            }
        }

        // Delete product
        public void Delete(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
