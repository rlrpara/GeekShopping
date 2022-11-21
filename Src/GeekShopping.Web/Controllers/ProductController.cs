using GeekShopping.Web.Models.filter;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.Web.Controllers
{
    public class ProductController : Controller
    {
        #region [Private Properties]
        private readonly IProductService _productService;
        #endregion

        #region [Private Methods]
        private static filterProductModel GetFilterProduct() =>  new filterProductModel
        {
            Name = "",
            Description = "",
            CategoryName = "",
            Price = 0,
            ActualPage = 1,
            QuantityPerPage = 50
        };
        #endregion

        #region [Constructor]
        public ProductController(IProductService productService) => _productService = productService;
        #endregion

        #region [Public Methods]
        public async Task<IActionResult> ProductIndex() => View(await _productService.GetAll(GetFilterProduct()));


        #endregion

        
    }
}
