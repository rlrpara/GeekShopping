using GeekShopping.Web.Models;
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
        public async Task<IActionResult> ProductCreate() => View();
        
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Insert(productModel);
                if(response) return RedirectToAction(nameof(ProductIndex));
            }
            return View(productModel);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            var product = await _productService.GetById(id);

            if(product is not null) return View(product);

            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.Update(model);
                if (response) return RedirectToAction(nameof(ProductIndex));
            }
            return View(model);
        }

        #endregion


    }
}
