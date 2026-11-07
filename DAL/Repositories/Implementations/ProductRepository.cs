using DAL.Contexte;
using DAL.Entities;
using eCommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    class ProductRepository : IProductRepositorycs
    {
        private readonly ApplicationDbContexte _context;
        public ProductRepository(ApplicationDbContexte contexte) { 
        _context=contexte;
        }
        public async Task<Product?> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            Product? product=await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if(product == null) return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _context.Products.FirstOrDefaultAsync(condition);
        }

        public async Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            return await _context.Products.Where(condition).ToListAsync();
        }

        public async Task<Product?> UpdateProduct(Product product)
        {
            Product? productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == product.ProductID);
            if (productToUpdate == null) return null;
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.Category = product.Category;
            productToUpdate.QuantityInStock = product.QuantityInStock;
            await _context.SaveChangesAsync();
            return productToUpdate;

        }
    }
}
