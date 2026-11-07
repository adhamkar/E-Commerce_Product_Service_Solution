using DAL.Contexte;
using DAL.Repositories.Implementations;
using eCommerce.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eCommerce.ProductsMicroservice.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services,IConfiguration configuration)
        {
            // Register your DAL services here
            // services.AddScoped<IYourRepository, YourRepository>();
            services.AddDbContext<ApplicationDbContexte>(opt =>{
                opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
            });
            services.AddScoped<IProductRepositorycs, ProductRepository>();
            return services;
        }
    }
}
