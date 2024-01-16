using FastStart.Repository;
using System.Data;
using System.Linq.Expressions;

namespace FastStart.Service
{
    public class BaseService<T> : IBaseService<T> where T : class, new()
    {
        #region 构造

        public BaseService(IBaseRepository<T> _baseRepository)
        {
            baseRepository = _baseRepository;
        }

        #endregion 构造

        protected readonly IBaseRepository<T> baseRepository;

        public Task<bool> CreateEntityAsync(T entity)
        {
            return baseRepository.CreateEntityAsync(entity);
        }

        public Task<int> CreateEntitysAsync(List<T> entitys)
        {
            return baseRepository.CreateEntitysAsync(entitys);
        }

        public Task<bool> DeleteEntityByIdAsync(object id)
        {
            return baseRepository.DeleteEntityByIdAsync(id);
        }

        public Task<int> DeleteEntitysByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.DeleteEntitysByWhereAsync(expression);
        }

        public Task<T> GetEntityByIdAsync(int id)
        {
            return baseRepository.GetEntityByIdAsync(id);
        }

        public Task<List<T>> GetEntitysAsync()
        {
            return baseRepository.GetEntitysAsync();
        }

        public Task<T> GetEntityByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.GetEntityByWhereAsync(expression);
        }

        public Task<List<T>> GetEntitysByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.GetEntitysByWhereAsync(expression);
        }

        public List<T> GetEntitysToPage(int pageNumber, int pageSize, ref int totalCount)
        {
            return baseRepository.GetEntitysToPage(pageNumber, pageSize, ref totalCount);
        }

        public Task<bool> UpdateEntityAsync(T entity)
        {
            return baseRepository.UpdateEntityAsync(entity);
        }

        public Task<int> UpdateEntitysAsync(List<T> entitys)
        {
            return baseRepository.UpdateEntitysAsync(entitys);
        }

        public Task<List<T>> GetEntitysBySqlListAsync(string sql)
        {
            return baseRepository.GetEntitysBySqlListAsync(sql);
        }

        public Task<DataTable> GetEntitysBySqlDatatableAsync(string sql)
        {
            return baseRepository.GetEntitysBySqlDatatableAsync(sql);
        }

        public Task<DataTable> UseProcAsync(string procName, object parameters)
        {
            return baseRepository.UseProcAsync(procName, parameters);
        }

        public void CreateModels(string name)
        {
            baseRepository.CreateModels(name);
        }
    }
}