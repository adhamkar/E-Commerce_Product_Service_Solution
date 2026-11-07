using BusinessLogic.DTO;
using DAL.Entities;
using System.Linq.Expressions;

namespace BusinessLogic.Services
{
    public interface IProductService
    {
        // Get all products
        Task<List<ProductResponse?>> GetAllProducts();
        // Add Product
        Task<ProductResponse?> AddProduct(ProductAddRequest newProduct);
        //update 
        Task<ProductResponse?> UpdateProduct(ProductUpdateRequest updatedProduct);
        //Delete
        Task<bool> DeleteProduct(Guid productId);
        //Get Products by condition
        Task<IEnumerable<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition);
        //Get Product by condition
        Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition);
    }
}
