using AutoMapper;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;

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
        /// 获取全部菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSysMenu")]
        public async Task<ResultModel<List<SysMenu>>> Get()
        {
            List<SysMenu> sysMenus = await sysMenuService.GetEntitysAsync();
            //List<SysMenuVO> sysMenuVOs = mapper.Map<List<SysMenu>>(sysMenus);
            return ResultModel<List<SysMenu>>.Success(sysMenus);
        }

        /// <summary>
        /// 分页获取菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysMenuToPage")]
        public ResultModel<List<SysMenu>> GetSysMenuToPage(int pageNumber = 1, int pageSize = 10)
        {
            int totalCount = 0;
            List<SysMenu> sysMenus = sysMenuService.GetEntitysToPage(pageNumber, pageSize, ref totalCount);
            return ResultModel<List<SysMenu>>.Success(sysMenus);
        }

        /// <summary>
        /// 通过id获取菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysMenuById")]
        public async Task<ResultModel<SysMenu>> GetSysMenuById(int id)
        {
            if (id == 0)
                return ResultModel<SysMenu>.Fail("参数不能为空");
            SysMenu sysMenu = await sysMenuService.GetEntityByWhereAsync(x => x.MenuId.Equals(id));
            return ResultModel<SysMenu>.Success(sysMenu);
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
            List<SysMenu> allSysMenus = await sysMenuService.GetEntitysAsync();

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

        /// <summary>
        ///
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMenuTreeByUserIdTest")]
        public ResultModel<List<SysMenuVO>> GetMenuTreeByUserIdTest(long UserId)
        {
            List<SysMenuVO> menus = new List<SysMenuVO>()
            {
                new SysMenuVO
                {
                    path = "/baseInfoManagement",
                    component = "#",
                    name = "BaseInfoManagement",
                    meta = new Meta
                    {
                        title = "router.baseInfoManagement",
                        icon = "carbon:skill-level-advanced"
                    },
                    children = new List<SysMenuVO>
                    {
                        new SysMenuVO
                        {
                            path = "systemSettings",
                            name = "SystemSettings",
                            component = "##",
                            meta = new Meta
                            {
                                title = "router.systemSettings"
                            },
                            children = new List<SysMenuVO>
                            {
                                new SysMenuVO
                                {
                                    path = "user",
                                    name = "User",
                                    component = "views/BaseInfoManagement/User/User",
                                    meta = new Meta
                                    {
                                        title = "router.user"
                                    }
                                },
                                new SysMenuVO
                                {
                                    path = "role",
                                    name = "Role",
                                    component = "views/BaseInfoManagement/Role/Role",
                                    meta = new Meta
                                    {
                                        title = "router.role"
                                    }
                                },
                                new SysMenuVO
                                {
                                    path = "menu",
                                    name = "Menu",
                                    component = "views/BaseInfoManagement/Menu/Menu",
                                    meta = new Meta
                                    {
                                        title = "router.menuManagement"
                                    }
                                },
                            }
                        }
                    }
                },
                new SysMenuVO
                {
                    path = "/WorkOrder",
                    component = "#",
                    name = "WorkOrder",
                    children = new List<SysMenuVO>
                    {
                        new SysMenuVO
                        {
                            path="Index",
                            component="views/BaseInfoManagement/WorkOrder/WorkOrder",
                            name="WorkOrderDemo",
                            meta=new Meta{
                                title="router.guide",
                                icon="cib:telegram-plane"
                            }
                        }
                    }
                }
            };
            return ResultModel<List<SysMenuVO>>.Success(menus);
        }

        private List<SysMenuVO> GetChildren(List<SysMenu> allSysMenus, long parentId)
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
        /// 添加菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSysMenu")]
        public async Task<ResultModel<bool>> AddSysMenu([FromBody] SysMenu sysMenu)
        {
            bool result = await sysMenuService.CreateEntityAsync(sysMenu);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 通过id删除单个菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSysMenu")]
        public async Task<ResultModel<bool>> DeleteSysMenu(int id)
        {
            if (id == 0)
                return ResultModel<bool>.Fail("参数不能为空");
            bool result = await sysMenuService.DeleteEntityByIdAsync(id);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 多选后通过ids进行批量删除
        /// </summary>
        /// <param name="ids">id数组</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSysMenuList")]
        public async Task<ResultModel<int>> DeleteSysMenuList([FromBody] object[] ids)
        {
            if (ids.Length <= 0)
                return ResultModel<int>.Fail("参数不能为空");
            int deletedCount = await sysMenuService.DeleteEntitysByWhereAsync(x => ids.Contains(x.MenuId));
            return ResultModel<int>.Success(deletedCount);
        }

        /// <summary>
        /// 更新菜单信息
        /// </summary>
        /// <param name="sysMenu">更新后的新实体信息</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateSysMenu")]
        public async Task<ResultModel<bool>> UpdateSysMenu([FromBody] SysMenu sysMenu)
        {
            bool result = await sysMenuService.UpdateEntityAsync(sysMenu);
            return ResultModel<bool>.Success(result);
        }
    }
}