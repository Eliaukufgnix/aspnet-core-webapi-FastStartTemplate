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
    /// 参数管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysConfig")]
    public class SysConfigController : ControllerBase
    {
        private readonly IBaseService<SysConfig> SysConfigService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_SysConfigService"></param>
        /// <param name="_mapper"></param>
        public SysConfigController(IBaseService<SysConfig> _SysConfigService, IMapper _mapper)
        {
            SysConfigService = _SysConfigService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysConfig>>> GetEntitiesByWhereToPageAsync([FromQuery] SysConfigDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysConfig>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await SysConfigService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.ConfigId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysConfig>>.Success(new SelectByPageVO<SysConfig>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysConfig sysConfig)
        {
            if (sysConfig == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysConfig.ConfigId != default ? await SysConfigService.UpdateEntityAsync(sysConfig) : await SysConfigService.CreateEntityAsync(sysConfig);
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
            int deletedCount = await SysConfigService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.ConfigId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}