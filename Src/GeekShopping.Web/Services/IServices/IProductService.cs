using GeekShopping.Web.Models;
using GeekShopping.Web.Models.filter;

namespace GeekShopping.Web.Services.IServices;

public interface IProductService
{
    Task<ProductModel> GetById(int codigo);
    Task<IEnumerable<ProductModel>?> GetAll(filterProductModel filtro);
    Task<bool> Insert(ProductModel model);
    Task<bool> Update(ProductModel model);
    Task<bool> RemoveById(int codigo);
}
