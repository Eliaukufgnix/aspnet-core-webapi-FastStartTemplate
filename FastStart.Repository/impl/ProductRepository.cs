using FastStart.Domain.Entity;
using SqlSugar;

namespace FastStart.Repository.impl
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ISqlSugarClient _db) : base(_db)
        {
        }
    }
}