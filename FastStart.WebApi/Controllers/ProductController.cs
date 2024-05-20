using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 产品管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_productService"></param>
        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        /// <summary>
        /// 通过类型获取产品信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductByType")]
        [AllowAnonymous]
        public async Task<ResultModel<List<Product>>> GetProductByType(string type)
        {
            List<Product> products = await productService.GetProductByType(type);
            return ResultModel<List<Product>>.Success(products);
        }
    }
}