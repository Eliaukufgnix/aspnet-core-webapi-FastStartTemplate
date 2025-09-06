﻿namespace FastStart.Domain.Constant
{
    public class HttpStatus
    {
        /// <summary>
        /// 操作成功
        /// </summary>

        public static int SUCCESS = 20000;

        /// <summary>
        /// 对象创建成功
        /// </summary>

        public static int CREATED = 201;

        /// <summary>
        /// 请求已经被接受
        /// </summary>

        public static int ACCEPTED = 202;

        /// <summary>
        /// 操作已经执行成功，但是没有返回数据
        /// </summary>

        public static int NO_CONTENT = 204;

        /// <summary>
        /// 资源已被移除
        /// </summary>

        public static int MOVED_PERM = 301;

        /// <summary>
        /// 重定向
        /// </summary>

        public static int SEE_OTHER = 303;

        /// <summary>
        /// 资源没有被修改
        /// </summary>

        public static int NOT_MODIFIED = 304;

        /// <summary>
        /// 参数列表错误（缺少，格式不匹配）
        /// </summary>

        public static int BAD_REQUEST = 400;

        /// <summary>
        /// 未授权
        /// </summary>

        public const int UNAUTHORIZED = 401;

        /// <summary>
        /// 访问受限，授权过期
        /// </summary>

        public static int FORBIDDEN = 403;

        /// <summary>
        /// 资源，服务未找到
        /// </summary>

        public static int NOT_FOUND = 404;

        /// <summary>
        /// 不允许的http方法
        /// </summary>

        public static int BAD_METHOD = 405;

        /// <summary>
        /// 资源冲突，或者资源被锁
        /// </summary>

        public static int CONFLICT = 409;

        /// <summary>
        /// 不支持的数据，媒体类型
        /// </summary>

        public static int UNSUPPORTED_TYPE = 415;

        /// <summary>
        /// 系统内部错误
        /// </summary>

        public static int ERROR = 500;

        /// <summary>
        /// 接口未实现
        /// </summary>

        public static int NOT_IMPLEMENTED = 501;

        /// <summary>
        /// 系统警告消息
        /// </summary>

        public static int WARN = 601;
    }
}