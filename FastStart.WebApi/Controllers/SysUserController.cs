using AutoMapper;
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
        private readonly ISysUserService sysUserService;
        private readonly IMapper mapper;

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
            Expressionable<SysUser> expressionable = Expressionable.Create<SysUser>();
            AddCondition(expressionable, queryParameters.UserId, x => x.UserId.ToString());
            AddCondition(expressionable, queryParameters.UserName, x => x.UserName.ToString());
            AddCondition(expressionable, queryParameters.NickName, x => x.NickName.ToString());
            AddCondition(expressionable, queryParameters.Sex, x => x.Sex.ToString());
            AddCondition(expressionable, queryParameters.Phonenumber, x => x.Phonenumber.ToString());
            AddCondition(expressionable, queryParameters.Email, x => x.Email.ToString());
            AddCondition(expressionable, queryParameters.CreateBy, x => x.CreateBy.ToString());
            AddCondition(expressionable, queryParameters.UpdateBy, x => x.UpdateBy.ToString());
            int totalCount = 0;
            List<SysUser> sysUsers = sysUserService.GetEntitysByWhereToPage(expressionable.ToExpression(), queryParameters.pageIndex, queryParameters.pageSize, ref totalCount);
            SelectByPageVO<SysUser> selectByPageVO = new(sysUsers, totalCount);
            return ResultModel<SelectByPageVO<SysUser>>.Success(selectByPageVO);
        }
        private void AddCondition<T>(Expressionable<T> expressionable, string parameter, Func<T, string> propertySelector) where T : class, new()
        {
            if (!string.IsNullOrEmpty(parameter))
            {
                expressionable.And(x => propertySelector(x).Contains(parameter));
            }
        }
        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSysUserById")]
        public async Task<ResultModel<SysUser>> GetSysUserById([FromQuery]string id)
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
        /// <param name="ids">id数组</param>
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
    }
}