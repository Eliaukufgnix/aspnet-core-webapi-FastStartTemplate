using FastStart.Domain.Entity;
using FastStart.Domain.Models;

namespace FastStart.Service
{
    public interface ISysMenuService : IBaseService<SysMenu>
    {
        Task<List<SysMenuVO>> GetMenuTreeByUserIdAsync(long UserId);
    }
}