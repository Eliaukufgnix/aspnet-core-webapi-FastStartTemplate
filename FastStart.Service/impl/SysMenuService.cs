using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Repository;

namespace FastStart.Service.impl
{
    public class SysMenuService : BaseService<SysMenu>, ISysMenuService
    {
        private readonly ISysMenuRepository sysMenuRepository;

        public SysMenuService(IBaseRepository<SysMenu> _baseRepository, ISysMenuRepository _sysMenuRepository) : base(_baseRepository)
        {
            sysMenuRepository = _sysMenuRepository;
        }

        public async Task<List<SysMenuVO>> GetMenuTreeByUserIdAsync(long UserId)
        {
            List<SysMenuDTO> sysMenuDTOs = UserId == 1 ? await sysMenuRepository.GetMenuTreeAll() : await sysMenuRepository.GetMenuTreeByUserId(UserId);
            List<SysMenuVO> sysMenuVOs = GetChildren(sysMenuDTOs, 0, 0);
            return sysMenuVOs;
        }

        private static List<SysMenuVO> GetChildren(List<SysMenuDTO> sysMenuDTOs, long ParentId, int level)
        {
            return sysMenuDTOs
                .Where(x => x.ParentId == ParentId) // 筛选出子菜单
                .Select(childMenu => new SysMenuVO
                {
                    path = ParentId == 0 ? "/" + childMenu.Path : childMenu.Path,
                    component = GetComponent(level, "views/" + childMenu.Component, sysMenuDTOs.Any(child => child.ParentId == childMenu.MenuId)),
                    redirect = childMenu.Redirect,
                    name = childMenu.MenuName,
                    meta = new Meta
                    {
                        icon = childMenu.Icon,
                        title = childMenu.MenuName
                    },
                    children = GetChildren(sysMenuDTOs, childMenu.MenuId, level + 1) // 递归获取子菜单的子菜单
                }).ToList();
        }

        private static string GetComponent(int level, string component, bool hasChildren)
        {
            if (hasChildren)
            {
                return new string('#', level + 1);
            }
            return component;
        }
    }
}