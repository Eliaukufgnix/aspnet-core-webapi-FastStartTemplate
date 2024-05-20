using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace FastStart.WebApi.Config
{
    /// <summary>
    /// Swagger配置
    /// </summary>
    public class SwaggerConfig
    {
        /// <summary>
        /// Swagger配置
        /// </summary>
        /// <param name="options"></param>
        public static void Configure(SwaggerGenOptions options)
        {
            // 1、配置Swagger文档的基本信息
            options.SwaggerDoc("File", new OpenApiInfo { Version = "v1", Title = "文件管理", Description = "文件管理" });
            options.SwaggerDoc("Login", new OpenApiInfo { Version = "v1", Title = "登录管理", Description = "登录管理" });
            options.SwaggerDoc("Product", new OpenApiInfo { Version = "v1", Title = "产品管理", Description = "产品管理" });
            options.SwaggerDoc("Quartz", new OpenApiInfo { Version = "v1", Title = "定时任务", Description = "Quartz定时任务" });
            options.SwaggerDoc("SysMenu", new OpenApiInfo { Version = "v1", Title = "菜单管理", Description = "菜单管理" });
            options.SwaggerDoc("SysUser", new OpenApiInfo { Version = "v1", Title = "用户管理", Description = "用户管理" });
            options.SwaggerDoc("SysRole", new OpenApiInfo { Version = "v1", Title = "角色管理", Description = "角色管理" });

            // 根据控制器标签分组
            options.DocInclusionPredicate((docName, apiDes) =>
            {
                if (!apiDes.TryGetMethodInfo(out MethodInfo method)) return false;
                //使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
                //(1).获取controller上的特性，注：method.DeclaringType只能获取controller上的特性
#pragma warning disable CS8602 // 解引用可能出现空引用。
                var controllerGroupNameList = method.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName).ToList();
#pragma warning restore CS8602 // 解引用可能出现空引用。
                if (docName == "Main" && !controllerGroupNameList.Any()) return true;
                //(2).获取action上的特性
                var actionGroupNameList = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                if (actionGroupNameList.Any())
                {
                    return actionGroupNameList.Any(u => u == docName);
                }
                return controllerGroupNameList.Any(u => u == docName);
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