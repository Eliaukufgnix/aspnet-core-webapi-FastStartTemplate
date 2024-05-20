using FastStart.Domain.Entity;
using FastStart.Domain.Models;

namespace FastStart.Repository
{
    public interface ISysUserRoleRepository : IBaseRepository<SysUserRole>
    {
        /// <summary>
        /// SysUserRole 和 SysUserQuery 连表查询获取用户信息
        /// </summary>
        /// <returns></returns>
        Task<TokenDTO> GetSysUserRoleLeftJoinSysUserQueryAsync(string UserName, string PassWord);
    }
}