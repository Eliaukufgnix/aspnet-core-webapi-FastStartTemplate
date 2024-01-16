using FastStart.Domain.Entity;
using SqlSugar;

namespace FastStart.Repository.impl
{
    public class SysUserRepository : BaseRepository<SysUser>, ISysUserRepository
    {
        public SysUserRepository(ISqlSugarClient _db) : base(_db)
        {
        }
    }
}