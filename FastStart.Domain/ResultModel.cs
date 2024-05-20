using Microsoft.AspNetCore.Http;

namespace FastStart.Domain
{
    #region 第一版

    /*public class AjaxResult : Dictionary<string, object>
    {
        // 状态码
        public static string CODE_TAG = "code";

        // 返回内容
        public static string MSG_TAG = "msg";

        // 数据对象
        public static string DATA_TAG = "data";

        /// <summary>
        /// 初始化一个新创建的 AjaxResult 对象，使其表示一个空消息。
        /// </summary>
        public AjaxResult()
        {
        }

        /// <summary>
        /// 初始化一个新创建的 AjaxResult 对象
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回内容</param>
        public AjaxResult(int code, string msg)
        {
            Add(CODE_TAG, code);
            Add(MSG_TAG, msg);
        }

        /// <summary>
        /// 初始化一个新创建的 AjaxResult 对象
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        public AjaxResult(int code, string msg, object data)
        {
            Add(CODE_TAG, code);
            Add(MSG_TAG, msg);
            if (data != null)
            {
                Add(DATA_TAG, data);
            }
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <returns>成功消息</returns>
        public static AjaxResult Success()
        {
            return Success("操作成功");
        }

        /// <summary>
        /// 返回成功数据
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static AjaxResult Success(object data)
        {
            return Success("操作成功", data);
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <returns>成功消息</returns>
        public static AjaxResult Success(string msg)
        {
            return Success(msg, null);
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static AjaxResult Success(string msg, object data)
        {
            return new AjaxResult(HttpStatus.SUCCESS, msg, data);
        }

        /// <summary>
        /// 返回警告消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <returns>警告消息</returns>
        public static AjaxResult Warn(string msg)
        {
            return Warn(msg, null);
        }

        /// <summary>
        /// 返回警告消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        /// <returns>警告消息</returns>
        public static AjaxResult Warn(string msg, object data)
        {
            return new AjaxResult(HttpStatus.WARN, msg, data);
        }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <returns>错误消息</returns>
        public static AjaxResult Error()
        {
            return Error("操作失败");
        }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <returns>错误消息</returns>
        public static AjaxResult Error(string msg)
        {
            return Error(msg, null);
        }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        /// <returns>错误消息</returns>
        public static AjaxResult Error(string msg, object data)
        {
            return new AjaxResult(HttpStatus.ERROR, msg, data);
        }

        /// <summary>
        /// 返回错误消息
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="msg">返回内容</param>
        /// <returns>错误消息</returns>
        public static AjaxResult Error(int code, string msg)
        {
            return new AjaxResult(code, msg, null);
        }

        /// <summary>
        /// 是否为成功消息
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSuccess()
        {
            return Equals(HttpStatus.SUCCESS, this[CODE_TAG]);
        }

        /// <summary>
        /// 是否为警告消息
        /// </summary>
        /// <returns>结果</returns>
        public bool IsWarn()
        {
            return Equals(HttpStatus.WARN, this[CODE_TAG]);
        }

        /// <summary>
        /// 是否为错误消息
        /// </summary>
        /// <returns>结果</returns>
        public bool IsError()
        {
            return Equals(HttpStatus.ERROR, this[CODE_TAG]);
        }

        /// <summary>
        /// 方便链式调用
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>数据对象</returns>
        public AjaxResult Put(string key, object value)
        {
            Add(key, value);
            return this;
        }
    }
    */

    #endregion 第一版

    /// <summary>
    /// 统一返回结果的格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        public const int SUCCESS = StatusCodes.Status200OK;
        public const int FAIL = StatusCodes.Status500InternalServerError;
        public int code { get; set; }
        public string message { get; set; }
        public T data { get; set; }
        public string timestamp { get; set; }

        #region 无参构造私有化

        private ResultModel()
        {
        }

        #endregion 无参构造私有化

        #region 请求成功

        public static ResultModel<T> Success()
        {
            return Result(SUCCESS, "请求成功", default);
        }

        public static ResultModel<T> Success(string message)
        {
            return Result(SUCCESS, message, default);
        }

        public static ResultModel<T> Success(T data)
        {
            return Result(SUCCESS, "请求成功", data);
        }

        public static ResultModel<T> Success(string message, T data)
        {
            return Result(SUCCESS, message, data);
        }

        public static ResultModel<T> Success(int code, string message, T data)
        {
            return Result(code, message, data);
        }

        #endregion 请求成功

        #region 请求失败

        public static ResultModel<T> Fail()
        {
            return Result(FAIL, "请求失败", default);
        }

        public static ResultModel<T> Fail(string message)
        {
            return Result(FAIL, message, default);
        }

        public static ResultModel<T> Fail(T data)
        {
            return Result(FAIL, "请求失败", data);
        }

        public static ResultModel<T> Fail(string message, T data)
        {
            return Result(FAIL, message, data);
        }

        public static ResultModel<T> Fail(int code, string message)
        {
            return Result(code, message, default);
        }

        public static ResultModel<T> Fail(int code, string message, T data)
        {
            return Result(code, message, data);
        }

        #endregion 请求失败

        private static ResultModel<T> Result(int code, string message, T data)
        {
            return new ResultModel<T>()
            {
                code = code,
                message = message,
                data = data,
                timestamp = DateTime.UtcNow.Ticks.ToString()
            };
        }
    }
}