using AutoMapper;
using BusinessLogic.DTO;
using BusinessLogic.RabbitMQ;
using BusinessLogic.RabbitMQ.Recordrs;
using DAL.Entities;
using eCommerce.DAL.Repositories.Interfaces;
using FluentValidation;
using System.Linq.Expressions;

namespace BusinessLogic.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepositorycs _productRepository;
        private readonly IValidator<ProductAddRequest> _productAddRequestValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateRequestValidator;
        private readonly IMapper _mapper;
        private readonly IPublisher _publisher;
        public ProductService(IProductRepositorycs productRepositorycs, IValidator<ProductAddRequest> productAddRequestValidator, 
            IValidator<ProductUpdateRequest> productUpdateRequestValidator, IMapper mapper, IPublisher publisher)
        {
            _productRepository = productRepositorycs;
            _productAddRequestValidator = productAddRequestValidator;
            _productUpdateRequestValidator = productUpdateRequestValidator;
            _mapper = mapper;
            _publisher = publisher;
        }
        public async Task<ProductResponse?> AddProduct(ProductAddRequest newProduct)
        {
            if (newProduct==null)
            {
                throw new ArgumentNullException(nameof(newProduct));
            }

            var validationResult=await _productAddRequestValidator.ValidateAsync(newProduct);
            if (!validationResult.IsValid)
            {
               string errors = string.Join(", ", validationResult.Errors.Select(
                   e => e.ErrorMessage));
                throw new ArgumentException($"Invalid product data: {errors}"); 
            }

            Product? product = _mapper.Map<Product>(newProduct);
            Product? productAdded=await _productRepository.AddProduct(product);

            if (productAdded==null)
            {
                return null;
            }
            // Publish event to RabbitMQ
            await _publisher.Pusblish("product.created", _mapper.Map<ProductResponse>(productAdded));
            return _mapper.Map<ProductResponse?>(productAdded);

        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            Product? product =await _productRepository.GetProductByCondition(p => p.ProductID == productId);
            if (product == null)
            {
                return false;
            }
            bool isDeleted = await _productRepository.DeleteProduct(productId);
            if(isDeleted)
                await _publisher.Pusblish("product.deleted", _mapper.Map<ProductResponse>(product));

            return isDeleted;
        }

        public async Task<List<ProductResponse?>> GetAllProducts()
        {
            IEnumerable<Product> products = await _productRepository.GetAllProducts();
            IEnumerable<ProductResponse?> mappedProducts = _mapper.Map<IEnumerable<ProductResponse?>>(products);
            await _publisher.Pusblish("product.listProducts", mappedProducts);

            return mappedProducts.ToList();
        }

        public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> condition)
        {
            Product? product = await _productRepository.GetProductByCondition(condition);
            if (product == null)
            {
                return null;
            }
            ProductResponse? result = _mapper.Map<ProductResponse?>(product);
            return result;
        }

        public async Task<IEnumerable<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> condition)
        {
            IEnumerable<Product> products = await _productRepository.GetProductsByCondition(condition);
            IEnumerable<ProductResponse?> mappedProducts = _mapper.Map<IEnumerable<ProductResponse?>>(products);
            return mappedProducts;
        }

        public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest updatedProduct)
        {
            Product? existingProduct = await _productRepository.GetProductByCondition(p => p.ProductID == updatedProduct.ProductID);

            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {updatedProduct.ProductID} does not exist.");
            }
            var validationResult = await _productUpdateRequestValidator.ValidateAsync(updatedProduct);

            if (!validationResult.IsValid)
            {
                string errors = string.Join(", ", validationResult.Errors.Select(
                    e => e.ErrorMessage));
                throw new ArgumentException($"Invalid product data: {errors}");
            }

            Product? product = _mapper.Map<Product>(updatedProduct);
            Product? productUpdated = await _productRepository.UpdateProduct(product);

            var result = _mapper.Map<ProductResponse?>(productUpdated);

            if (result != null)
            {
                await _publisher.Pusblish("product.updatedProduct", new ProductUpdate(result.ProductID, result.ProductName));
            }

            return result;
        }
    }
}
