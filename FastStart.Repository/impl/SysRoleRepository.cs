using FastStart.Domain.Entity;
using SqlSugar;

namespace FastStart.Repository.impl
{
    public class SysRoleRepository : BaseRepository<SysRole>, ISysRoleRepository
    {
        public SysRoleRepository(ISqlSugarClient _db) : base(_db)
        {
        }
    }
}