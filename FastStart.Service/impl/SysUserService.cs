using FastStart.Domain.Entity;
using FastStart.Repository;

namespace FastStart.Service.impl
{
    public class SysUserService : BaseService<SysUser>, ISysUserService
    {
        public SysUserService(IBaseRepository<SysUser> _baseRepository) : base(_baseRepository)
        {
        }
    }
}