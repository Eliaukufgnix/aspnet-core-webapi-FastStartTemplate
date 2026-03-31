namespace FastStart.Common.Exception
{
    [Serializable]
    public class BaseException : System.Exception
    {
        /// <summary>
        /// 所属模块
        /// </summary>
        public string? Module { get; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 错误码对应的参数
        /// </summary>
        public object[]? Args { get; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string? DefaultMessage { get; }

        public BaseException(string? module, int code, object[]? args, string? defaultMessage) : base(defaultMessage)
        {
            Module = module;
            Code = code;
            Args = args;
            DefaultMessage = defaultMessage;
        }

        public BaseException(string? module, int code, string? defaultMessage)
            : this(module, code, null, defaultMessage)
        {
        }

        public BaseException(string? module, int code, object[]? args)
            : this(module, code, args, null)
        {
        }

        public BaseException(string? module, string? defaultMessage)
            : this(module, default, null, defaultMessage)
        {
        }

        public BaseException(int code, string? defaultMessage)
            : this(null, code, null, defaultMessage)
        {
        }

        public BaseException(int code, object[]? args)
            : this(null, code, args, null)
        {
        }

        public BaseException(string? defaultMessage)
            : this(null, default, null, defaultMessage)
        {
        }
    }
}