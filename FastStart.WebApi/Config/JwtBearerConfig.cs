using FastStart.Domain;
using FastStart.Domain.Constant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// JwtBearer配置信息
    /// </summary>
    public class JwtBearerConfig
    {
        public static void Configure(JwtBearerOptions options)
        {
            // token验证相关设置
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true, //是否验证Issuer
                ValidIssuer = JWTConstants.Issuer, //发行人Issuer
                ValidateAudience = true, //是否验证Audience
                ValidAudience = JWTConstants.Audience, //订阅人Audience
                ValidateIssuerSigningKey = true, //是否验证SecurityKey
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConstants.Secret)), //SecurityKey
                ValidateLifetime = true, //是否验证失效时间
                ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                RequireExpirationTime = true,
            };
            // 自定义认证失败的响应，CustomerExceptionFilter 只能拦截控制器中的异常，针对 JWT 再额外进行自定义认证失败的响应
            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    // 跳过默认的处理逻辑，因为我们要返回自定义的响应数据
                    context.HandleResponse();

                    // 定义返回类型
                    int code = StatusCodes.Status401Unauthorized;
                    ResultModel<List<object>> result = ResultModel<List<object>>.Fail(code, "认证失败");

                    // 设置返回内容
                    context.Response.StatusCode = StatusCodes.Status200OK; // 仍然返回200状态码
                    context.Response.ContentType = "application/json;charset=utf-8";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }
            };
        }
    }
}
