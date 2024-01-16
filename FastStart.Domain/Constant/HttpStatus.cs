namespace FastStart.Domain.Constant
{
    public class HttpStatus
    {
        //操作成功

        public static int SUCCESS = 20000;

        //对象创建成功

        public static int CREATED = 201;

        //请求已经被接受

        public static int ACCEPTED = 202;

        //操作已经执行成功，但是没有返回数据

        public static int NO_CONTENT = 204;

        //资源已被移除

        public static int MOVED_PERM = 301;

        //重定向

        public static int SEE_OTHER = 303;

        //资源没有被修改

        public static int NOT_MODIFIED = 304;

        //参数列表错误（缺少，格式不匹配）

        public static int BAD_REQUEST = 400;

        //未授权

        public const int UNAUTHORIZED = 401;

        //访问受限，授权过期

        public static int FORBIDDEN = 403;

        //资源，服务未找到

        public static int NOT_FOUND = 404;

        //不允许的http方法

        public static int BAD_METHOD = 405;

        //资源冲突，或者资源被锁

        public static int CONFLICT = 409;

        //不支持的数据，媒体类型

        public static int UNSUPPORTED_TYPE = 415;

        //系统内部错误

        public static int ERROR = 500;

        //接口未实现

        public static int NOT_IMPLEMENTED = 501;

        //系统警告消息

        public static int WARN = 601;
    }
}