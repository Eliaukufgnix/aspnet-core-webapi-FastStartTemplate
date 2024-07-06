using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

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
        public async Task<ResultModel<List<Product>>> GetProductByType(string type)
        {
            Expressionable<Product> expressionable = Expressionable.Create<Product>();
            if (!string.IsNullOrEmpty(type))
            {
                expressionable.And(x => x.Type.ToString() == type);
            }
            List<Product> products = await productService.GetEntitysByWhereAsync(expressionable.ToExpression());
            return ResultModel<List<Product>>.Success(products);
        }
    }
}