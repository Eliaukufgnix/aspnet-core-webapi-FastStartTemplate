namespace FastStart.Domain.Constant
{
    public class Constants
    {
        /// <summary>
        /// UTF-8 字符集
        /// </summary>
        public static string UTF8 = "UTF-8";

        /// <summary>
        /// GBK 字符集
        /// </summary>
        public static string GBK = "GBK";

        /// <summary>
        /// www主域
        /// </summary>
        public static string WWW = "www.";

        /// <summary>
        /// http请求
        /// </summary>
        public static string HTTP = "http://";

        /// <summary>
        /// https请求
        /// </summary>
        public static string HTTPS = "https://";

        /// <summary>
        /// 通用成功标识
        /// </summary>
        public static string SUCCESS = "0";

        /// <summary>
        /// 通用失败标识
        /// </summary>
        public static string FAIL = "1";

        /// <summary>
        /// 登录成功
        /// </summary>
        public static string LOGIN_SUCCESS = "Success";

        /// <summary>
        /// 注销
        /// </summary>
        public static string LOGOUT = "Logout";

        /// <summary>
        /// 注册
        /// </summary>
        public static string REGISTER = "Register";

        /// <summary>
        /// 登录失败
        /// </summary>
        public static string LOGIN_FAIL = "Error";

        /// <summary>
        /// 所有权限标识
        /// </summary>

        public static string ALL_PERMISSION = "*:*:*";

        /// <summary>
        /// 管理员角色权限标识
        /// </summary>
        public static string SUPER_ADMIN = "admin";

        /// <summary>
        /// 角色权限分隔符
        /// </summary>
        public static string ROLE_DELIMETER = ",";

        /// <summary>
        /// 权限标识分隔符
        /// </summary>
        public static string PERMISSION_DELIMETER = ",";

        /// <summary>
        /// 验证码有效期（分钟）
        /// </summary>
        public static int CAPTCHA_EXPIRATION = 2;

        /// <summary>
        /// 令牌
        /// </summary>
        public static string TOKEN = "token";

        /// <summary>
        /// 令牌前缀
        /// </summary>
        public static string TOKEN_PREFIX = "Bearer ";

        /// <summary>
        /// 令牌前缀
        /// </summary>
        public static string LOGIN_USER_KEY = "login_user_key";

        /// <summary>
        /// 用户ID
        /// </summary>
        public static string JWT_USERID = "userid";

        /// <summary>
        /// 用户名称
        /// </summary>
        public static string JWT_USERNAME = "username";

        /// <summary>
        /// 用户头像
        /// </summary>
        public static string JWT_AVATAR = "avatar";

        /// <summary>
        /// 创建时间
        /// </summary>
        public static string JWT_CREATED = "created";

        /// <summary>
        /// 用户权限
        /// </summary>
        public static string JWT_AUTHORITIES = "authorities";
    }
}