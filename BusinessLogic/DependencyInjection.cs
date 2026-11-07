using BusinessLogic.Mapper;
using BusinessLogic.RabbitMQ;
using BusinessLogic.Services;
using BusinessLogic.Services.Implementations;
using BusinessLogic.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace eCommerce.ProductsMicroservice.BusinessLogic
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
        {
            // Register your BL services here
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IPublisher,Publisher>();

            return services;
        }
    }
}
