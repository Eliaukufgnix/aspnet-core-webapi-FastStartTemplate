using FastStart.Domain.Entity;
using FastStart.Repository;

namespace FastStart.Service.impl
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(IBaseRepository<Product> _baseRepository) : base(_baseRepository)
        {
        }
    }
}