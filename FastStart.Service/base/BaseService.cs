using FastStart.Repository;
using SqlSugar;
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

        public IBaseRepository<T> baseRepository;

        public Task<bool> CreateEntityAsync(T entity)
        {
            return baseRepository.CreateEntityAsync(entity);
        }

        public Task<int> CreateEntitiesAsync(List<T> entitys)
        {
            return baseRepository.CreateEntitysAsync(entitys);
        }

        public Task<bool> DeleteEntityByIdAsync(object id)
        {
            return baseRepository.DeleteEntityByIdAsync(id);
        }

        public Task<int> DeleteEntitiesByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.DeleteEntitysByWhereAsync(expression);
        }

        public Task<T> GetEntityByIdAsync(object id)
        {
            return baseRepository.GetEntityByIdAsync(id);
        }

        public Task<List<T>> GetEntitiesAsync()
        {
            return baseRepository.GetEntitysAsync();
        }

        public Task<T> GetEntityByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.GetEntityByWhereAsync(expression);
        }

        public Task<List<T>> GetEntitiesByWhereAsync(Expression<Func<T, bool>> expression)
        {
            return baseRepository.GetEntitysByWhereAsync(expression);
        }

        public List<T> GetEntitiesToPage(int pageIndex, int pageSize, ref int totalCount)
        {
            return baseRepository.GetEntitysToPage(pageIndex, pageSize, ref totalCount);
        }

        public List<T> GetEntitiesByWhereToPage(Expression<Func<T, bool>> expression, int pageIndex, int pageSize, ref int totalCount)
        {
            return baseRepository.GetEntitiesByWhereToPage(expression, pageIndex, pageSize, ref totalCount);
        }

        public async Task<List<T>> GetEntitiesByWhereToPageAsync(Expression<Func<T, bool>> expression, int pageIndex, int pageSize, RefAsync<int> totalCount)
        {
            return await baseRepository.GetEntitiesByWhereToPageAsync(expression, pageIndex, pageSize, totalCount);
        }

        public async Task<List<T>> GetEntitiesByWhereToPageAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderByExpression, int pageIndex, int pageSize, RefAsync<int> totalCount, bool isAsc = false)
        {
            return await baseRepository.GetEntitiesByWhereToPageAsync(expression, orderByExpression, pageIndex, pageSize, totalCount, isAsc);
        }

        public Task<bool> UpdateEntityAsync(T entity)
        {
            return baseRepository.UpdateEntityAsync(entity);
        }

        public Task<int> UpdateEntitiesAsync(List<T> entitys)
        {
            return baseRepository.UpdateEntitysAsync(entitys);
        }

        public Task<List<T>> GetEntitiesBySqlListAsync(string sql)
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