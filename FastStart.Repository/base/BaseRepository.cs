using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace FastStart.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, new()
    {
        private readonly ISqlSugarClient db;

        public BaseRepository(ISqlSugarClient _db)
        {
            db = _db;
        }

        public virtual async Task<bool> CreateEntityAsync(T entity)
        {
            try
            {
                return await db.Insertable(entity).ExecuteCommandAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> CreateEntitysAsync(List<T> entitys)
        {
            try
            {
                return await db.Insertable(entitys).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<bool> DeleteEntityByIdAsync(object id)
        {
            try
            {
                return await db.Deleteable<T>().In(id).ExecuteCommandAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> DeleteEntitysByWhereAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await db.Deleteable<T>().Where(expression).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual int DeleteEntitysBatch(List<T> list)
        {
            try
            {
                return db.Deleteable<T>(list).ExecuteCommand();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> GetEntityByIdAsync(int id)
        {
            try
            {
                return await db.Queryable<T>().InSingleAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetEntitysAsync()
        {
            try
            {
                return await db.Queryable<T>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<T> GetEntityByWhereAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await db.Queryable<T>().Where(expression).SingleAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetEntitysByWhereAsync(Expression<Func<T, bool>> expression)
        {
            try
            {
                return await db.Queryable<T>().Where(expression).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual List<T> GetEntitysToPage(int pageNumber, int pageSize, ref int totalCount)
        {
            try
            {
                return db.Queryable<T>().ToPageList(pageNumber, pageSize, ref totalCount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<bool> UpdateEntityAsync(T entity)
        {
            try
            {
                return await db.Updateable(entity).ExecuteCommandAsync() > 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<int> UpdateEntitysAsync(List<T> entitys)
        {
            try
            {
                return await db.Updateable(entitys).ExecuteCommandAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetEntitysBySqlListAsync(string sql)
        {
            try
            {
                return await db.Ado.SqlQueryAsync<T>(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<DataTable> GetEntitysBySqlDatatableAsync(string sql)
        {
            try
            {
                return await db.Ado.GetDataTableAsync(sql);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<DataTable> UseProcAsync(string procName, object parameters)
        {
            try
            {
                return await db.Ado.UseStoredProcedure().GetDataTableAsync(procName, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CreateModels(string name = "1")
        {
            if (name == "1")
            {
                db.DbFirst.IsCreateAttribute().CreateClassFile(@"E:\VS2022Projects\FastStart\FastStart.Domain\Entity\", "FastStart.Domain.Entity");
            }
            else
            {
                db.DbFirst.IsCreateAttribute().Where(name).CreateClassFile(@"E:\VS2022Projects\FastStart\FastStart.Domain\Entity\", "FastStart.Domain.Entity");
            }
        }
    }
}