using System.Data;
using System.Linq.Expressions;

namespace FastStart.Repository
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// 1条新增
        /// </summary>
        /// <param name="entity">要创建的实体对象</param>
        /// <returns>如果成功创建实体记录，则返回 true；否则返回 false。</returns>
        Task<bool> CreateEntityAsync(T entity);

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entities">要新增的实体对象集合</param>
        /// <returns>新增操作影响的记录条数</returns>
        Task<int> CreateEntitysAsync(List<T> entitys);

        /// <summary>
        /// 1条修改
        /// </summary>
        /// <param name="entity">要修改的实体对象</param>
        /// <returns>如果成功修改实体记录，则返回 true；否则返回 false。</returns>
        Task<bool> UpdateEntityAsync(T entity);

        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entities">要修改的实体对象集合</param>
        /// <returns>受影响的记录数</returns>

        Task<int> UpdateEntitysAsync(List<T> entitys);

        /// <summary>
        /// 1条删除
        /// </summary>
        /// <param name="id">要删除的记录的主键ID。</param>
        /// <returns>删除操作影响的记录数。</returns>
        Task<bool> DeleteEntityByIdAsync(object id);

        /// <summary>
        /// 根据条件批量删除
        /// </summary>
        /// <param name="expression">条件</param>
        /// <returns>返回受影响条数</returns>
        Task<int> DeleteEntitysByWhereAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        int DeleteEntitysBatch(List<T> list);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns>返回实体集合</returns>
        Task<List<T>> GetEntitysAsync();

        /// <summary>
        /// 根据ID单条查询
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <returns>查询到的实体</returns>
        Task<T> GetEntityByIdAsync(int id);

        /// <summary>
        /// 根据条件单条查询
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>符合条件的实体</returns>
        Task<T> GetEntityByWhereAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 根据条件批量查询
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns>符合条件的实体集合</returns>
        Task<List<T>> GetEntitysByWhereAsync(Expression<Func<T, bool>> expression);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNumber">页码，从1开始</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="totalCount">总记录数</param>
        /// <returns>查询到的实体集合</returns>
        List<T> GetEntitysToPage(int pageNumber, int pageSize, ref int totalCount);

        /// <summary>
        /// 原生SQL语句查询-List
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>查询到的实体集合</returns>
        Task<List<T>> GetEntitysBySqlListAsync(string sql);

        /// <summary>
        /// 原生SQL查询-DataTable
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <returns>查询到的DataTable</returns>
        Task<DataTable> GetEntitysBySqlDatatableAsync(string sql);

        /// <summary>
        /// 使用存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns>返回Datatable</returns>
        Task<DataTable> UseProcAsync(string procName, object parameters);

        /// <summary>
        /// 生成实体
        /// </summary>
        void CreateModels(string name);
    }
}