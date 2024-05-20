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
            // 如果异常没有被处理则进行处理
            if (context.ExceptionHandled == false)
            {
                // 记录异常信息和堆栈跟踪
                StringBuilder exMsg = new();
                exMsg.AppendLine();
                exMsg.AppendLine($"【异常方法:】{context.HttpContext.Request.Path}");
                exMsg.AppendLine($"【请求类型:】{context.HttpContext.Request.Method}");
                exMsg.AppendLine($"【异常错误:】{context.Exception.Message}");
                exMsg.AppendLine($"【堆栈跟踪:】{context.Exception.StackTrace}");
                Log.Error(exMsg.ToString());
                // 定义返回类型
                int code = context.Exception is FastStart.Common.Exception.BaseException baseException ? baseException.Code : default;
                ResultModel<List<object>> result = code == 0 ? ResultModel<List<object>>.Fail(context.Exception.Message) : ResultModel<List<object>>.Fail(code, context.Exception.Message);
                context.Result = new ContentResult
                {
                    // 返回状态码设置为200，表示成功
                    StatusCode = StatusCodes.Status200OK,
                    // 设置返回格式
                    ContentType = "application/json;charset=utf-8",
                    Content = JsonConvert.SerializeObject(result)
                };
            }
            // 设置为true，表示异常已经被处理了
            context.ExceptionHandled = true;
            // 关闭并刷新 Serilog
            Log.CloseAndFlush();
        }
    }
}