using AutoMapper;
using FastStart.Domain.Entity;
using FastStart.Domain.Models;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// AutoMapper配置信息
    /// </summary>
    public class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            // 1、建立映射关系,左侧是原类型，右侧是目标类型
            //cfg.CreateMap<FuncDTO, Func>();
            //cfg.CreateMap<FuncUserGroupDTO, FuncUserGroup>();
            // 其他映射关系...
            cfg.CreateMap<SysUserVO, SysUser>();
            cfg.CreateMap<SysUser, SysUserVO>();
            cfg.CreateMap<SysMenu, SysMenuVO>();
            // 2、使用时通过 IMapper 依赖注入
            // private readonly IMapper _mapper;
            //public MyController(IMapper mapper)
            //{
            //    _mapper = mapper;
            //}
            // _mapper.Map<目标对象类型>(源对象信息);
        }
    }
}