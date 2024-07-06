using FastStart.Domain.Entity;
using FastStart.Repository;

namespace FastStart.Service.impl
{
    public class SysRoleService : BaseService<SysRole>, ISysRoleService
    {
        public SysRoleService(IBaseRepository<SysRole> _baseRepository) : base(_baseRepository)
        {
        }
    }
}