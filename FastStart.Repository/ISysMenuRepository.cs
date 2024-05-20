using FastStart.Domain.Entity;
using FastStart.Domain.Models;

namespace FastStart.Repository
{
    public interface ISysMenuRepository : IBaseRepository<SysMenu>
    {
        Task<List<SysMenuDTO>> GetMenuTreeAll();

        Task<List<SysMenuDTO>> GetMenuTreeByUserId(long UserId);
    }
}