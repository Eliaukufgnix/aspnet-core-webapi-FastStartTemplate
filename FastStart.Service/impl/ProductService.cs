using FastStart.Domain.Entity;
using FastStart.Repository;
using SqlSugar;

namespace FastStart.Service.impl
{
    public class ProductService : BaseService<Product>, IProductService
    {
        private readonly IBaseRepository<Product> productRepository;

        public ProductService(IBaseRepository<Product> _baseRepository) : base(_baseRepository)
        {
            productRepository  = _baseRepository;
        }

        public Task<List<Product>> GetProductByType(string type)
        {
            Expressionable<Product> expressionable = Expressionable.Create<Product>();
            if (!string.IsNullOrEmpty(type))
            {
                expressionable.And(x => x.Type != null && x.Type.ToString() == type);
            }
            Task<List<Product>> task = productRepository.GetEntitysByWhereAsync(expressionable.ToExpression());
            return task;
        }
    }
}