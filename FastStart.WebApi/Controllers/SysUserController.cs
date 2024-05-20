using AutoMapper;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Service;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq.Expressions;

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
        private readonly ISysUserService sysUserService;
        private readonly IMapper mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sysUserService"></param>
        /// <param name="_mapper"></param>
        public SysUserController(ISysUserService _sysUserService, IMapper _mapper)
        {
            sysUserService = _sysUserService;
            mapper = _mapper;
        }

        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSysUser")]
        public async Task<ResultModel<List<SysUserVO>>> Get()
        {
            List<SysUser> sysUsers = await sysUserService.GetEntitysAsync();
            List<SysUserVO> sysUserVOs = mapper.Map<List<SysUserVO>>(sysUsers);
            return ResultModel<List<SysUserVO>>.Success(sysUserVOs);
        }

        /// <summary>
        /// 带条件分页查询获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysUserByWhereToPage")]
        public ResultModel<SelectByPageVO<SysUser>> GetSysUserByWhereToPage([FromQuery] SysUserDTO queryParameters)
        {
            // Build the query expression
            Expression<Func<SysUser, bool>> whereExpression = BuildQueryExpression(queryParameters);

            // Now pass the expression to your service layer
            int totalCount = 0;
            List<SysUser> sysUsers = sysUserService.GetEntitysByWhereToPage(
                whereExpression,
                queryParameters.pageIndex,
                queryParameters.pageSize,
                ref totalCount
            );

            SelectByPageVO<SysUser> selectByPageVO = new(sysUsers, totalCount);
            return ResultModel<SelectByPageVO<SysUser>>.Success(selectByPageVO);
        }

        /// <summary>
        /// 构建查询表达式
        /// </summary>
        /// <param name="queryParameters"></param>
        /// <returns></returns>
        private static Expression<Func<SysUser, bool>> BuildQueryExpression(SysUserDTO queryParameters)
        {
            var expressionable = Expressionable.Create<SysUser>();
            if (!string.IsNullOrEmpty(queryParameters.UserName))
            {
                expressionable.And(x => x.UserName.Contains(queryParameters.UserName));
            }
            if (!string.IsNullOrEmpty(queryParameters.NickName))
            {
                expressionable.And(x => x.NickName.Contains(queryParameters.NickName));
            }
            if (!string.IsNullOrEmpty(queryParameters.Phonenumber))
            {
                expressionable.And(x => x.Phonenumber.Contains(queryParameters.Phonenumber));
            }

            if (!string.IsNullOrEmpty(queryParameters.Email))
            {
                expressionable.And(x => x.Email.Contains(queryParameters.Email));
            }
            if (!string.IsNullOrEmpty(queryParameters.Sex))
            {
                expressionable.And(x => x.Sex == queryParameters.Sex);
            }
            return expressionable.ToExpression();
        }

        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysUserById")]
        public async Task<ResultModel<SysUser>> GetSysUserById([FromQuery] string id)
        {
            if (string.IsNullOrEmpty(id))
                return ResultModel<SysUser>.Fail("参数不能为空");
            SysUser sysUser = await sysUserService.GetEntityByWhereAsync(x => x.UserId.Equals(id));
            return ResultModel<SysUser>.Success(sysUser);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("AddSysUser")]
        public async Task<ResultModel<bool>> AddSysUser([FromBody] SysUser sysUser)
        {
            bool result = await sysUserService.CreateEntityAsync(sysUser);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 通过id删除单个用户信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSysUser")]
        public async Task<ResultModel<bool>> DeleteSysUser(object id)
        {
            if (id == null)
                return ResultModel<bool>.Fail("参数不能为空");
            bool result = await sysUserService.DeleteEntityByIdAsync(id);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 多选后通过ids进行批量删除
        /// </summary>
        /// <param name="dto">选择的实体</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSysUserList")]
        public async Task<ResultModel<int>> DeleteSysUserList([FromBody] IdsDTO dto)
        {
            if (dto.Ids == null || dto.Ids.Length <= 0)
                return ResultModel<int>.Fail("参数不能为空");
            int deletedCount = await sysUserService.DeleteEntitysByWhereAsync(x => dto.Ids.Contains(x.UserId));
            return ResultModel<int>.Success(deletedCount);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="sysUser">更新后的新实体信息</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateSysUser")]
        public async Task<ResultModel<bool>> UpdateSysUser([FromBody] SysUser sysUser)
        {
            bool result = await sysUserService.UpdateEntityAsync(sysUser);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSysUser")]
        public async Task<ResultModel<bool>> SaveSysUser([FromBody] SysUser sysUser)
        {
            if (sysUser == null)
            {
                return ResultModel<bool>.Fail("参数不能为空");
            }
            bool result = sysUser.UserId != default ? await sysUserService.UpdateEntityAsync(sysUser) : await sysUserService.CreateEntityAsync(sysUser);
            return ResultModel<bool>.Success(result);
        }
    }
}