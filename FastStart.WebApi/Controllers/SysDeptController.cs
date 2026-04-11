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
    [ApiExplorerSettings(GroupName = "SysDept")]
    public class SysDeptController : ControllerBase
    {
        private readonly IBaseService<SysDept> SysDeptService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_SysDeptService"></param>
        /// <param name="_mapper"></param>
        public SysDeptController(IBaseService<SysDept> _SysDeptService, IMapper _mapper)
        {
            SysDeptService = _SysDeptService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysDept>>> GetEntitiesByWhereToPageAsync([FromQuery] SysDeptDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysDept>(queryParameters);
            var totalCountRef = new RefAsync<int>(0);
            var data = await SysDeptService.GetEntitiesByWhereToPageAsync(
                where,
                x => x.DeptId,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCountRef,
                false
            );
            var totalCount = totalCountRef.Value;
            return ResultModel<SelectByPageVO<SysDept>>.Success(new SelectByPageVO<SysDept>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysDept sysDept)
        {
            if (sysDept == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysDept.DeptId != default ? await SysDeptService.UpdateEntityAsync(sysDept) : await SysDeptService.CreateEntityAsync(sysDept);
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
            int deletedCount = await SysDeptService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.DeptId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}