using AutoMapper;
using FastStart.Common.Utils;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 岗位管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "SysPost")]
    public class SysPostController : ControllerBase
    {
        private readonly IBaseService<SysPost> SysPostService;
        private readonly IMapper mapper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="_SysPostService"></param>
        /// <param name="_mapper"></param>
        public SysPostController(IBaseService<SysPost> _SysPostService, IMapper _mapper)
        {
            SysPostService = _SysPostService;
            mapper = _mapper;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetEntitiesByWhereToPageAsync")]
        public async Task<ResultModel<SelectByPageVO<SysPost>>> GetEntitiesByWhereToPageAsync([FromQuery] SysPostDTO queryParameters)
        {
            var where = QueryExpressionBuilder.BuildExpression<SysPost>(queryParameters);
            var totalCount = 0;
            var data = await SysPostService.GetEntitiesByWhereToPageAsync(
                where,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                totalCount
            );
            return ResultModel<SelectByPageVO<SysPost>>.Success(new SelectByPageVO<SysPost>(data, totalCount));
        }

        /// <summary>
        /// 保存数据（新增/修改）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveEntity")]
        public async Task<ResultModel<bool>> SaveEntity([FromBody] SysPost sysPost)
        {
            if (sysPost == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysPost.PostId != default ? await SysPostService.UpdateEntityAsync(sysPost) : await SysPostService.CreateEntityAsync(sysPost);
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
            int deletedCount = await SysPostService.DeleteEntitiesByWhereAsync(x => dto.Ids.Contains(x.PostId));
            return ResultModel<int>.Success(deletedCount);
        }
    }
}