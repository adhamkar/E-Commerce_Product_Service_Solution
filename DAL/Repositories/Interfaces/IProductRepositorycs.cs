using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.DAL.Repositories.Interfaces;

    public interface IProductRepositorycs
    {
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductsByCondition(Expression<Func<Product, bool>> condition);
    Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> condition);
    Task<Product?> AddProduct(Product product);
    Task<Product?> UpdateProduct(Product product);
    Task<bool> DeleteProduct(Guid productId);
}

