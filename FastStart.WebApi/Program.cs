using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using FastStart.WebApi.Config;
using FastStart.WebApi.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog.Events;
using Serilog;

namespace FastStart.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ///*
            // Serilog WriteTo.File详解
            //    path：默认路径是程序的bin目录+path参数，当然也可以写绝对路径，只需要写入参数就可以了
            //    outputTemplate：日志模板，可以自定义
            //    rollingInterval：创建文件的类别，可以是分钟，小时，天，月。 此参数可以让创建的log文件名 + 时间。例如log-20240520.txt
            //    fileSizeLimitBytes： 单个文件最大长度
            //    retainedFileCountLimit：设置日志文件个数最大值，默认31，意思就是只保留最近的31个日志文件
            //    rollOnFileSizeLimit： 是否限制单个文件的最大大小。true：超过fileSizeLimitBytes设置的最大值，开启新文件记录。false：不在记录，等下一个滚动间隔再记录。
            //    retainedFileTimeLimit：日志保存时间，删除过期日志
            // */
            //Log.Logger = new LoggerConfiguration()
            //.WriteTo.File(
            //    //每天一个文件 ,生成类似：errorlog-20240520.txt
            //    $"logs\\errorlog-.txt",
            //        // outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {NewLine}{Version}{myval}"
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {NewLine}{Version}{myval}",
            //        // 指定滚动间隔，RollingInterval.Day 每天一个新文件
            //        rollingInterval: RollingInterval.Day,
            //        // fileSizeLimitBytes: null // 文件限制大小：无
            //        fileSizeLimitBytes: 52428800, // 50MB
            //                                      // rollOnFileSizeLimit true：超过fileSizeLimitBytes设置的最大值，开启新文件记录。false：不在记录，等下一个滚动间隔再记录。
            //        rollOnFileSizeLimit: true,
            //        // retainedFileCountLimit: null 保留的文件数量  如果不配置默认只保留31个
            //        retainedFileCountLimit: 180
            //    )
            //    .CreateLogger();
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            // 这个是过滤一些不想看到的日志
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Logger(lc => lc
                                .Filter.ByIncludingOnly(e => e.Properties.ContainsKey("SqlLog"))
                                .WriteTo.File(
                                    $"Logs\\Sql\\sql-log-.txt",
                                    rollingInterval: RollingInterval.Day,
                                    fileSizeLimitBytes: 52428800, // 50MB
                                    rollOnFileSizeLimit: true,
                                    retainedFileCountLimit: 180,
                                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Properties}{NewLine}"
                                ))
            .WriteTo.Logger(lc => lc
                                .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal)
                                .WriteTo.File(
                                    $"Logs\\Exceptions\\exception-log-.txt", 
                                    rollingInterval: RollingInterval.Day,
                                    fileSizeLimitBytes: 52428800,
                                    retainedFileCountLimit: 180
                                ))
            .CreateLogger();
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();
                // Add services to the container.

                builder.Services.AddControllers(options =>
                {
                    // 自定义一个一场拦截器，使响应结果能返回为指定格式。但该拦截器只能拦截控制器中的异常。
                    options.Filters.Add(new CustomerExceptionFilter());
                });
                // 全局验权，要求每个控制器下的方法都要进行token验证，如果想关闭，方法上添加 [AllowAnonymous] 即可。
                builder.Services.AddRazorPages().AddMvcOptions(option =>
                {
                    option.Filters.Add(new AuthorizeFilter());
                });
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                //注册服务
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    // CustomerExceptionFilter 只能拦截控制器中的异常，针对 JWT 再额外进行自定义认证失败的响应
                    JwtBearerConfig.Configure(options);
                });

                // Swagger服务注册抽离成类
                builder.Services.AddSwaggerGen(options =>
                {
                    SwaggerConfig.Configure(options);
                });
                // 注册AutoMapper
                builder.Services.AddAutoMapper(cfg =>
                {
                    AutoMapperConfig.Configure(cfg);
                });
                // 覆盖默认的容器工厂。
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacConfig());
                });

                // 配置跨域
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin() // 允许所有Origin策略

                               // 允许所有请求方法：Get,Post,Put,Delete
                               .AllowAnyMethod()

                               // 允许所有请求头:application/json
                               .AllowAnyHeader();
                    });
                });
                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }
                app.UseSerilogRequestLogging();
                // 允许所有跨域，cors是在ConfigureServices方法中配置的跨域策略名称
                app.UseCors("CorsPolicy");
                app.UseHttpsRedirection();
                // 认证
                app.UseAuthentication();
                // 授权
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "意外终止");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}