using AutoMapper;
using FastStart.Domain;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;
using FastStart.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastStart.WebApi.Controllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    [Route("dev-api/[controller]")]
    [ApiController]
    public class SysUserController : ControllerBase
    {
        private readonly ISysUserService service;
        private readonly IMapper mapper;

        public SysUserController(ISysUserService _service, IMapper _mapper)
        {
            service = _service;
            mapper = _mapper;
        }

        /// <summary>
        /// 获取全部用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllSysUser")]
        public async Task<ResultModel<List<SysUserVO>>> Get()
        {
            List<SysUser> sysUsers = await service.GetEntitysAsync();
            List<SysUserVO> sysUserVOs = mapper.Map<List<SysUserVO>>(sysUsers);
            return ResultModel<List<SysUserVO>>.Success(sysUserVOs);
        }

        /// <summary>
        /// 分页获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSysUserToPage")]
        public ResultModel<List<SysUser>> GetSysUserToPage(int pageNumber = 1, int pageSize = 10)
        {
            int totalCount = 0;
            List<SysUser> sysUsers = service.GetEntitysToPage(pageNumber, pageSize, ref totalCount);
            return ResultModel<List<SysUser>>.Success(sysUsers);
        }

        /// <summary>
        /// 通过id获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetSysUserById")]
        public async Task<ResultModel<SysUser>> GetSysUserById(int id)
        {
            SysUser sysUser = await service.GetEntityByWhereAsync(x => x.UserId.Equals(id));
            return ResultModel<SysUser>.Success(sysUser);
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("AddSysUser")]
        public async Task<ResultModel<bool>> AddSysUser([FromBody] SysUser sysUser)
        {
            bool result = await service.CreateEntityAsync(sysUser);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 通过id删除单个用户信息
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteSysUser")]
        public async Task<ResultModel<bool>> DeleteSysUser(object id)
        {
            bool result = await service.DeleteEntityByIdAsync(id);
            return ResultModel<bool>.Success(result);
        }

        /// <summary>
        /// 多选后通过ids进行批量删除
        /// </summary>
        /// <param name="ids">id数组</param>
        /// <returns></returns>
        [HttpDelete]
        [AllowAnonymous]
        [Route("DeleteSysUserList")]
        public async Task<ResultModel<int>> DeleteSysUserList([FromBody] object[] ids)
        {
            int deletedCount = await service.DeleteEntitysByWhereAsync(x => ids.Contains(x.UserId));
            return ResultModel<int>.Success(deletedCount);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="sysUser">更新后的新实体信息</param>
        /// <returns></returns>
        [HttpPut]
        [AllowAnonymous]
        [Route("UpdateSysUser")]
        public async Task<ResultModel<bool>> UpdateSysUser([FromBody] SysUser sysUser)
        {
            bool result = await service.UpdateEntityAsync(sysUser);
            return ResultModel<bool>.Success(result);
        }
    }
}