using GeekShopping.Web.Models;
using GeekShopping.Web.Models.filter;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class ProductService : IProductService
{
    #region [Private Properties]
    private readonly HttpClient _httpClient;
    #endregion
    #region [Public Methods]
    public const string _basePath = "api/v1/product";
    #endregion

    #region [Constructor]
    public ProductService(HttpClient httpClient) => _httpClient = httpClient;
    #endregion

    #region [Public Methods]
    public async Task<IEnumerable<ProductModel>?> GetAll(filterProductModel filtro)
    {
        var response = await _httpClient.PostAsJson($"{_basePath}/getall", filtro);
        var result = await response.ReadContentAs<ApiResult<ProductModel>>();

        return result.dados;
    }
    public async Task<ProductModel> GetById(int codigo)
    {
        var response = await _httpClient.GetAsync($"{_basePath}/getbyid/{codigo}");
        var teste = await response.ReadContentAs<ApiResult<ProductModel>>();
        return teste.dados.FirstOrDefault();
    }
    public async Task<bool> Insert(ProductModel model)
    {
        var response = await _httpClient.PostAsJson($"{_basePath}/insert", model);
        return await response.ReadContentAs<bool>();
    }
    public async Task<bool> Update(ProductModel model)
    {
        var response = await _httpClient.PutAsJson($"{_basePath}/update", model);
        return await response.ReadContentAs<bool>();
    }
    public async Task<bool> RemoveById(int codigo)
    {
        var response = await _httpClient.DeleteAsync($"{_basePath}/remove/{codigo}");
        return await response.ReadContentAs<bool>();
    }
    
    #endregion
}
