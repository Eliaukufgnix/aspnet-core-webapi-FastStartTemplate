using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using SqlSugar;

namespace FastStart.Repository.impl
{
    public class SysUserRoleRepository : BaseRepository<SysUserRole>, ISysUserRoleRepository
    {
        public SysUserRoleRepository(ISqlSugarClient _db) : base(_db)
        {
        }

        public async Task<TokenDTO> GetSysUserRoleLeftJoinSysUserQueryAsync(string UserName, string PassWord)
        {
            try
            {
                return await db.Queryable<SysUserRole>()
                    .LeftJoin<SysUser>((x, y) => x.UserId == y.UserId)
                    .Where((x, y) => y.UserName == UserName && y.Password == PassWord)
                    .Select((x, y) => new TokenDTO { UserId = y.UserId, UserName = y.UserName, RoleId = x.RoleId })
                    .FirstAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}