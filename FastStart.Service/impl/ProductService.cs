using FastStart.Domain.Entity;
using FastStart.Repository;
using SqlSugar;

namespace FastStart.Service.impl
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IBaseRepository<Product> _baseRepository, IProductRepository _productRepository) : base(_baseRepository)
        {
            productRepository = _productRepository;
        }

        public Task<List<Product>> GetProductByType(string type)
        {
            Expressionable<Product> expressionable = Expressionable.Create<Product>();
            if (!string.IsNullOrEmpty(type))
            {
                expressionable.And(x => x.Type.ToString() == type);
            }
            Task<List<Product>> task = productRepository.GetEntitysByWhereAsync(expressionable.ToExpression());
            return task;
        }
    }
}