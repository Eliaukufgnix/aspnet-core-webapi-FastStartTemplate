using AutoMapper;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysRole")]
    public class SysRoleController : ControllerBase
    {
        private readonly ISysRoleService sysRoleService;
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sysRoleService"></param>
        /// <param name="_mapper"></param>
        public SysRoleController(ISysRoleService _sysRoleService, IMapper _mapper)
        {
            sysRoleService = _sysRoleService;
            mapper = _mapper;
        }

        /// <summary>
        /// 获取全部角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSysRole")]
        public async Task<ResultModel<List<SysRole>>> Get()
        {
            List<SysRole> sysMenus = await sysRoleService.GetEntitysAsync();
            //List<SysRoleVO> sysMenuVOs = mapper.Map<List<SysRole>>(sysMenus);
            return ResultModel<List<SysRole>>.Success(sysMenus);
        }
    }
}