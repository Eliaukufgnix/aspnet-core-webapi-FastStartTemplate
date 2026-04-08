using AutoMapper;
using FastStart.Common.Utils;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Domain.Models.DTO;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysMenu")]
    public class SysMenuController : ControllerBase
    {
        private readonly ISysMenuService sysMenuService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_sysMenuService"></param>
        /// <param name="_mapper"></param>
        public SysMenuController(ISysMenuService _sysMenuService, IMapper _mapper)
        {
            sysMenuService = _sysMenuService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysMenu>>> GetEntitiesByWhereToPageAsync([FromQuery] SysMenuDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysMenu>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await sysMenuService.GetEntitiesByWhereToPageAsync(
                where,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef
            );
            data = data.OrderByDescending(x => x.OrderNum).ToList();
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysMenu>>.Success(new SelectByPageVO<SysMenu>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysMenu sysMenu)
        {
            if (sysMenu == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysMenu.MenuId != default ? await sysMenuService.UpdateEntityAsync(sysMenu) : await sysMenuService.CreateEntityAsync(sysMenu);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 多选后通过ids进行批量删除
        /// </summary>
        /// <param name="dto">选择的实体</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteEntitiesAsync")]
        public async Task<ResultModel<int>> DeleteEntitiesAsync([FromBody] IdsDTO dto)
        {
            if (dto.Ids == null || dto.Ids.Length <= 0)
                return ResultModel<int>.Fail("参数不能为空");
            int deletedCount = await sysMenuService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.MenuId));
            return ResultModel<int>.Success(deletedCount);
        }

        /// <summary>
        /// 获取树形菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysMenu")]
        public async Task<ResultModel<List<SysMenuVO>>> GetSysMenu()
        {
            // 一次性获取所有菜单项
            List<SysMenu> allSysMenus = await sysMenuService.GetEntitiesAsync();

            // 构建树状结构
            var sysMenuVOs = allSysMenus
                .Where(x => x.ParentId == 0) // 筛选出顶级菜单
                .Select(parentMenu => new SysMenuVO
                {
                    path = parentMenu.Path,
                    component = "#",
                    redirect = parentMenu.Component,
                    name = parentMenu.MenuName,
                    meta = new Meta
                    {
                        icon = parentMenu.Icon,
                        title = parentMenu.MenuName
                    },
                    children = GetChildren(allSysMenus, parentMenu.MenuId)
                }).ToList();

            return ResultModel<List<SysMenuVO>>.Success(sysMenuVOs);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="allSysMenus"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private static List<SysMenuVO> GetChildren(List<SysMenu> allSysMenus, long parentId)
        {
            return allSysMenus
                .Where(x => x.ParentId == parentId) // 筛选出子菜单
                .Select(childMenu => new SysMenuVO
                {
                    path = childMenu.Path,
                    component = "##",
                    redirect = childMenu.Component,
                    name = childMenu.MenuName,
                    meta = new Meta
                    {
                        icon = childMenu.Icon,
                        title = childMenu.MenuName
                    },
                    children = GetChildren(allSysMenus, childMenu.MenuId) // 递归获取子菜单的子菜单
                }).ToList();
        }

        /// <summary>
        /// 通过用户id获取菜单信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuTreeByUserId")]
        public async Task<ResultModel<List<SysMenuVO>>> GetMenuTreeByUserId(long UserId)
        {
            List<SysMenuVO> menus = await sysMenuService.GetMenuTreeByUserIdAsync(UserId);
            return ResultModel<List<SysMenuVO>>.Success(menus);
        }
    }
}