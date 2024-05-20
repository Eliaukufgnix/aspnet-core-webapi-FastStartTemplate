using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using SqlSugar;

namespace FastStart.Repository.impl
{
    public class SysMenuRepository : BaseRepository<SysMenu>, ISysMenuRepository
    {
        public SysMenuRepository(ISqlSugarClient _db) : base(_db)
        {
        }

        public Task<List<SysMenuDTO>> GetMenuTreeAll()
        {
            try
            {
                return db.Queryable<SysMenu>()
                    .Where((m) => new[] { "M", "C" }.Contains(m.MenuType))
                    .OrderBy(m => new { m.ParentId, m.OrderNum })
                    .Select((m) => new SysMenuDTO
                    {
                        MenuId = m.MenuId,
                        ParentId = m.ParentId,
                        MenuName = m.MenuName,
                        Path = m.Path,
                        Component = m.Component,
                        Redirect = m.Redirect,
                        Query = m.Query,
                        Visible = m.Visible,
                        Status = m.Status,
                        Perms = SqlFunc.IsNull(m.Perms, ""),
                        IsFrame = m.IsFrame,
                        IsCache = m.IsCache,
                        MenuType = m.MenuType,
                        Icon = m.Icon,
                        OrderNum = m.OrderNum,
                        CreateTime = m.CreateTime
                    })
                    .Distinct()
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<SysMenuDTO>> GetMenuTreeByUserId(long UserId)
        {
            try
            {
                return db.Queryable<SysMenu, SysRoleMenu, SysUserRole, SysRole, SysUser>((m, rm, ur, ro, u) => new JoinQueryInfos(
                                JoinType.Left, m.MenuId == rm.MenuId,
                                JoinType.Left, rm.RoleId == ur.RoleId,
                                JoinType.Left, ur.RoleId == ro.RoleId,
                                JoinType.Left, ur.UserId == u.UserId
                            ))
                            .Where((m, rm, ur, ro, u) => u.UserId == UserId && new[] { "M", "C" }.Contains(m.MenuType) && m.Status == "0" && ro.Status == "0")
                            .OrderBy((m, rm, ur, ro, u) => new { m.ParentId, m.OrderNum })
                            .Select((m, rm, ur, ro, u) => new SysMenuDTO
                            {
                                MenuId = m.MenuId,
                                ParentId = m.ParentId,
                                MenuName = m.MenuName,
                                Path = m.Path,
                                Component = m.Component,
                                Redirect = m.Redirect,
                                Query = m.Query,
                                Visible = m.Visible,
                                Status = m.Status,
                                Perms = SqlFunc.IsNull(m.Perms, ""),
                                IsFrame = m.IsFrame,
                                IsCache = m.IsCache,
                                MenuType = m.MenuType,
                                Icon = m.Icon,
                                OrderNum = m.OrderNum,
                                CreateTime = m.CreateTime
                            })
                            .Distinct()
                            .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}