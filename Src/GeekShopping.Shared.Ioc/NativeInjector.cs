using GeekShoping.Shared.Services.Interface;
using GeekShoping.Shared.Services.Service;
using GeekShopping.Shared.Data.Repositories;
using GeekShopping.Shared.Domain.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace GeekShopping.Shared.Ioc;

public static class NativeInjector
{
    public static void RegisterServices(this IServiceCollection services)
    {
        #region Services
        services.AddTransient<IProductService, ProductService>();

        #endregion

        #region Repositories
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IBaseRepository, BaseRepository>();
        #endregion
    }
}
