namespace FastStart.Domain.Constant
{
    public class UserConstants
    {
        /// <summary>
        /// 平台内系统用户的唯一标志
        /// </summary>
        public static string SYS_USER = "SYS_USER";

        /// <summary>
        /// 正常状态
        ///  </summary>
        public static string NORMAL = "0";

        /// <summary>
        /// 异常状态
        /// </summary>
        public static string EXCEPTION = "1";

        /// <summary>
        /// 用户封禁状态
        /// </summary>
        public static string USER_DISABLE = "1";

        /// <summary>
        /// 角色封禁状态
        /// </summary>
        public static string ROLE_DISABLE = "1";

        /// <summary>
        /// 部门正常状态
        /// </summary>
        public static string DEPT_NORMAL = "0";

        /// <summary>
        /// 部门停用状态
        /// </summary>
        public static string DEPT_DISABLE = "1";

        /// <summary>
        /// 字典正常状态
        /// </summary>
        public static string DICT_NORMAL = "0";

        /// <summary>
        /// 是否为系统默认（是）
        /// </summary>
        public static string YES = "Y";

        /// <summary>
        /// 是否菜单外链（是）
        /// </summary>
        public static string YES_FRAME = "0";

        /// <summary>
        /// 是否菜单外链（否）
        /// </summary>
        public static string NO_FRAME = "1";

        /// <summary>
        /// 菜单类型（目录）
        /// </summary>
        public static string TYPE_DIR = "M";

        /// <summary>
        /// 菜单类型（菜单）
        /// </summary>
        public static string TYPE_MENU = "C";

        /// <summary>
        /// 菜单类型（按钮）
        /// </summary>
        public static string TYPE_BUTTON = "F";

        /// <summary>
        /// Layout组件标识
        /// </summary>
        public static string LAYOUT = "Layout";

        /// <summary>
        /// ParentView组件标识
        /// </summary>
        public static string PARENT_VIEW = "ParentView";

        /// <summary>
        /// InnerLink组件标识
        /// </summary>
        public static string INNER_LINK = "InnerLink";

        /// <summary>
        /// 校验是否唯一的返回标识
        /// </summary>
        public static bool UNIQUE = true;

        public static bool NOT_UNIQUE = false;

        /// <summary>
        /// 用户名长度限制
        /// </summary>
        public static int USERNAME_MIN_LENGTH = 2;

        public static int USERNAME_MAX_LENGTH = 20;

        /// <summary>
        /// 密码长度限制
        /// </summary>
        public static int PASSWORD_MIN_LENGTH = 5;

        public static int PASSWORD_MAX_LENGTH = 20;
    }
}