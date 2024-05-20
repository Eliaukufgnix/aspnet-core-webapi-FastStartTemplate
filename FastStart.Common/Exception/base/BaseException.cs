namespace FastStart.Common.Exception
{
    [Serializable]
    public class BaseException : System.Exception
    {
        /// <summary>
        /// 所属模块
        /// </summary>
        public string Module { get; }

        /// <summary>
        /// 错误码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 错误码对应的参数
        /// </summary>
        public object[] Args { get; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string DefaultMessage { get; }

        public BaseException(string module, int code, object[] args, string defaultMessage)
            : base(defaultMessage)
        {
            Module = module;
            Code = code;
            Args = args;
            DefaultMessage = defaultMessage;
        }

        public BaseException(string module, int code, string defaultMessage)
            : this(module, code, null, defaultMessage)
        {
        }

        public BaseException(string module, int code, object[] args)
            : this(module, code, args, null)
        {
        }

        public BaseException(string module, string defaultMessage)
            : this(module, default, null, defaultMessage)
        {
        }

        public BaseException(int code, string defaultMessage)
            : this(null, code, null, defaultMessage)
        {
        }

        public BaseException(int code, object[] args)
            : this(null, code, args, null)
        {
        }

        public BaseException(string defaultMessage)
            : this(null, default, null, defaultMessage)
        {
        }

        /// <summary>
        /// 序列化构造函数（必须的，为了支持序列化）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BaseException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            Module = info.GetString(nameof(Module));
            Code = info.GetInt32(nameof(Code));
            Args = (object[])info.GetValue(nameof(Args), typeof(object[]));
            DefaultMessage = info.GetString(nameof(DefaultMessage));
        }

        /// <summary>
        /// 序列化方法（必须的，为了支持序列化）
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Module), Module);
            info.AddValue(nameof(Code), Code);
            info.AddValue(nameof(Args), Args);
            info.AddValue(nameof(DefaultMessage), DefaultMessage);
        }
    }
}