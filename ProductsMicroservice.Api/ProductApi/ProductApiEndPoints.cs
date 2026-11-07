using BusinessLogic.DTO;
using BusinessLogic.Services;
using FluentValidation;

namespace ProductsMicroservice.Api.ProductApi
{
    public static class ProductApiEndPoints
    {
        public static IEndpointRouteBuilder MapProductApiEndPoints(this IEndpointRouteBuilder app)
        {
            // Get All routing - api/products/all
            app.MapGet("/api/products/all", async (IProductService productService) =>
            {
                List<ProductResponse?> products = await productService.GetAllProducts();
                return Results.Ok(products);
            });

            //Get By ID
            app.MapGet("/api/products/product/id/{ProductId:Guid}", async (IProductService productService, Guid ProductId) =>
            {
                ProductResponse? product = await productService.GetProductByCondition(p => p.ProductID == ProductId);
                return Results.Ok(product);
            });

            //Get products by quantity of stock
            app.MapGet("/api/products/quantityStock/{QuantityStock:int}", async (IProductService productService, int QuantityStock) =>
            {
                var products=await productService.GetProductsByCondition(p=>p.QuantityInStock == QuantityStock);
                return Results.Ok(products);
            });

            //Get products by Name
            app.MapGet("/api/products/name/{SearchParam}", async (IProductService productService, string SearchParam) =>
            {
                var productsByName = await productService.GetProductsByCondition(p => p.ProductName.Contains(SearchParam, StringComparison.OrdinalIgnoreCase) 
                                                                            && p.ProductName!=null);
                var productsByCategory = await productService.GetProductsByCondition(p => p.Category.Contains(SearchParam, StringComparison.OrdinalIgnoreCase)
                                                                            && p.Category != null);
                var products = productsByName.Union(productsByCategory);
                return Results.Ok(products);
            });

            //delete by id 
            app.MapDelete("/api/products/delete/id/{ProductId:Guid}", async (IProductService productService, Guid ProductId) =>
            {
                bool isDeleted = await productService.DeleteProduct(ProductId);
                if (isDeleted)
                {
                    return Results.Ok($"Product with ID {ProductId} has been deleted.");
                }
                else
                {
                    return Results.NotFound($"Product with ID {ProductId} not found.");
                }
            });

            //Add Product
            app.MapPost("/api/products/add", async (IProductService productService, ProductAddRequest newProduct,IValidator<ProductAddRequest> requestValidator) =>
            {
                var validationResult = await requestValidator.ValidateAsync(newProduct);
                if (!validationResult.IsValid)
                {
                    Dictionary< string,string[]> errors = 
                    validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(grp=>grp.Key,grp=>grp
                    .Select(e=>e.ErrorMessage)
                    .ToArray());
                    
                    return Results.ValidationProblem(errors);
                }
                ProductResponse? addedProduct = await productService.AddProduct(newProduct);
                
                if (addedProduct == null)
                {
                    return Results.Problem("Failed to add the product. Please check the provided data.");
                }
                return Results.Created($"/api/products/product/id/{addedProduct.ProductID}", addedProduct);
            });

            //Update Product
            app.MapPut("/api/products/update/{ProductId:Guid}", async (IProductService productService, ProductUpdateRequest updatedProduct, Guid ProductId) =>
            {
                ProductResponse? existingProduct=await productService.GetProductByCondition(p => p.ProductID == ProductId);
                if (existingProduct == null)
                {
                    return Results.NotFound($"Product with ID {ProductId} does not exist.");
                }
                ProductResponse? product = await productService.UpdateProduct(updatedProduct);
                return Results.Ok(product);
            });

            return app;
        }
    }
}
