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
    /// 角色管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysRole")]
    public class SysRoleController : ControllerBase
    {
        private readonly IBaseService<SysRole> sysRoleService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_sysRoleService"></param>
        /// <param name="_mapper"></param>
        public SysRoleController(IBaseService<SysRole> _sysRoleService, IMapper _mapper)
        {
            sysRoleService = _sysRoleService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysRole>>> GetEntitiesByWhereToPageAsync([FromQuery] SysRoleDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysRole>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await sysRoleService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.RoleId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysRole>>.Success(new SelectByPageVO<SysRole>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysRole sysRole)
        {
            if (sysRole == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysRole.RoleId != default ? await sysRoleService.UpdateEntityAsync(sysRole) : await sysRoleService.CreateEntityAsync(sysRole);
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
            {
                return ResultModel<int>.Fail("参数不能为空");
            }
            int deletedCount = await sysRoleService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.RoleId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}