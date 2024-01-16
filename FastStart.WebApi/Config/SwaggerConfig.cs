using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public class SwaggerConfig
    {
        public static void Configure(SwaggerGenOptions options)
        {
            // 1、配置Swagger文档的基本信息
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "WebApi",
                Description = "WebApi项目文档"
            });

            // 2、编写注释时无效，可进行一下配置（前提是在项目的属性->生成->输出中设置好开启.xml文档生成，并制定好位置和名称）
            var file = Path.Combine(AppContext.BaseDirectory, "swagger.xml");// xml文档绝对路径
            var path = Path.Combine(AppContext.BaseDirectory, file);// xml文档绝对路径
            options.IncludeXmlComments(path, true);// true : 显示控制器层注释
            //options.OrderActionsBy(o => o.RelativePath);// 对action的名称进行排序，如果有多个，就可以看见效果了。

            // 3、配置Swagger可以发送带Token的请求
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "请在字段中输入单词“Bearer”，后跟空格和JWT值。",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference()
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        }
    }
}
