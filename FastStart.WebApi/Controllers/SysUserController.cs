using AutoMapper;
using FastStart.Common.Utils;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysUser")]
    public class SysUserController : ControllerBase
    {
        private readonly IBaseService<SysUser> sysUserService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_sysUserService"></param>
        /// <param name="_mapper"></param>
        public SysUserController(IBaseService<SysUser> _sysUserService, IMapper _mapper)
        {
            sysUserService = _sysUserService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysUser>>> GetEntitiesByWhereToPageAsync([FromQuery] SysUserDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysUser>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await sysUserService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.UserId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysUser>>.Success(new SelectByPageVO<SysUser>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysUser sysUser)
        {
            if (sysUser == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysUser.UserId != default ? await sysUserService.UpdateEntityAsync(sysUser) : await sysUserService.CreateEntityAsync(sysUser);
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
            int deletedCount = await sysUserService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.UserId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}