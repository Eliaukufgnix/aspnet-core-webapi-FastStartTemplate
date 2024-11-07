using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Repository;
using SqlSugar;

namespace FastStart.Service.impl
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductService productService;

        public ProductService(IBaseRepository<Product> _baseRepository, IProductService _productService) : base(_baseRepository)
        {
            productService = _productService;
        }

        public Task<List<Product>> GetProductByType(string type)
        {
            Expressionable<Product> expressionable = Expressionable.Create<Product>();
            if (!string.IsNullOrEmpty(type))
            {
                expressionable.And(x => x.Type.ToString() == type);
            }
            Task<List<Product>> task = productService.GetEntitysByWhereAsync(expressionable.ToExpression());
            return task;
        }
    }
}