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
    /// 部门管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysDict")]
    public class SysDictController : ControllerBase
    {
        private readonly IBaseService<SysDictType> SysDictService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_sysDictService"></param>
        /// <param name="_mapper"></param>
        public SysDictController(IBaseService<SysDictType> _sysDictService, IMapper _mapper)
        {
            SysDictService = _sysDictService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysDictType>>> GetEntitiesByWhereToPageAsync([FromQuery] SysDictDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysDictType>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await SysDictService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.DictId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysDictType>>.Success(new SelectByPageVO<SysDictType>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysDictType sysDictType)
        {
            if (sysDictType == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysDictType.DictId != default ? await SysDictService.UpdateEntityAsync(sysDictType) : await SysDictService.CreateEntityAsync(sysDictType);
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
            int deletedCount = await SysDictService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.DictId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}