using FastStart.Domain.Entity;

namespace FastStart.Domain.Models
{
    public class LoginUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long userId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long deptId { get; set; }

        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string token { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public long loginTime { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expireTime { get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string ipaddr { get; set; }

        /// <summary>
        /// 登录地点
        /// </summary>
        public string loginLocation { get; set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string os { get; set; }

        /// <summary>
        /// 权限列表
        /// </summary>
        public List<string> permissions { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public SysUser user { get; set; }
    }
}