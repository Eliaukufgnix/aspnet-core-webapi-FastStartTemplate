using FastStart.Common.Exceptions;
using FastStart.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace FastStart.WebApi.Filter
{
    /// <summary>
    /// 全局自定义异常过滤器
    /// </summary>
    public class GlobalCustomerExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 重写OnExceptionAsync方法，定义自己的处理逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        void IExceptionFilter.OnException(ExceptionContext context)
        {
            // 使用 Serilog 的 Error 方法重载，传入整个异常对象（会自动递归输出 InnerException）
            Log.Error(context.Exception,
                "全局异常捕获 | 路径: {Path} | 方法: {Method}",
                context.HttpContext.Request.Path,
                context.HttpContext.Request.Method);

            // 可选：同时输出到控制台，便于调试
            Console.WriteLine(context.Exception.ToString());

            int code = context.Exception is BaseException baseException ? baseException.Code : 0;
            ResultModel<List<object>> result = code == 0
                ? ResultModel<List<object>>.Fail(context.Exception.Message)
                : ResultModel<List<object>>.Fail(code, context.Exception.Message);

            context.Result = new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "application/json;charset=utf-8",
                Content = JsonConvert.SerializeObject(result)
            };

            context.ExceptionHandled = true;
        }
    }
}