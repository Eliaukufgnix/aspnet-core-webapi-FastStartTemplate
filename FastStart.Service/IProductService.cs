using FastStart.Domain.Entity;

namespace FastStart.Service
{
    public interface IProductService : IBaseService<Product>
    {
        Task<List<Product>> GetProductByType(string type);
    }
}