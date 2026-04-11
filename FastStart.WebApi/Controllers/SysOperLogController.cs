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
    /// 操作日志管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysOperLog")]
    public class SysOperLogController : ControllerBase
    {
        private readonly IBaseService<SysOperLog> SysOperLogService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_SysOperLogService"></param>
        /// <param name="_mapper"></param>
        public SysOperLogController(IBaseService<SysOperLog> _SysOperLogService, IMapper _mapper)
        {
            SysOperLogService = _SysOperLogService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysOperLog>>> GetEntitiesByWhereToPageAsync([FromQuery] SysOperLogDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysOperLog>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await SysOperLogService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.OperId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysOperLog>>.Success(new SelectByPageVO<SysOperLog>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysOperLog SysOperLog)
        {
            if (SysOperLog == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = SysOperLog.OperId != default ? await SysOperLogService.UpdateEntityAsync(SysOperLog) : await SysOperLogService.CreateEntityAsync(SysOperLog);
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
            int deletedCount = await SysOperLogService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.OperId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}