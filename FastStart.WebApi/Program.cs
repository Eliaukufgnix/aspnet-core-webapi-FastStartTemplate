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
            // Serilog WriteTo.File���
            //    path��Ĭ��·���ǳ����binĿ¼+path��������ȻҲ����д����·����ֻ��Ҫд������Ϳ�����
            //    outputTemplate����־ģ�壬�����Զ���
            //    rollingInterval�������ļ�����𣬿����Ƿ��ӣ�Сʱ���죬�¡� �˲��������ô�����log�ļ��� + ʱ�䡣����log-20240520.txt
            //    fileSizeLimitBytes�� �����ļ���󳤶�
            //    retainedFileCountLimit��������־�ļ��������ֵ��Ĭ��31����˼����ֻ���������31����־�ļ�
            //    rollOnFileSizeLimit�� �Ƿ����Ƶ����ļ�������С��true������fileSizeLimitBytes���õ����ֵ���������ļ���¼��false�����ڼ�¼������һ����������ټ�¼��
            //    retainedFileTimeLimit����־����ʱ�䣬ɾ��������־
            // */
            //Log.Logger = new LoggerConfiguration()
            //.WriteTo.File(
            //    //ÿ��һ���ļ� ,�������ƣ�errorlog-20240520.txt
            //    $"logs\\errorlog-.txt",
            //        // outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {NewLine}{Version}{myval}"
            //        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception} {NewLine}{Version}{myval}",
            //        // ָ�����������RollingInterval.Day ÿ��һ�����ļ�
            //        rollingInterval: RollingInterval.Day,
            //        // fileSizeLimitBytes: null // �ļ����ƴ�С����
            //        fileSizeLimitBytes: 52428800, // 50MB
            //                                      // rollOnFileSizeLimit true������fileSizeLimitBytes���õ����ֵ���������ļ���¼��false�����ڼ�¼������һ����������ټ�¼��
            //        rollOnFileSizeLimit: true,
            //        // retainedFileCountLimit: null �������ļ�����  ���������Ĭ��ֻ����31��
            //        retainedFileCountLimit: 180
            //    )
            //    .CreateLogger();
            Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
            // ����ǹ���һЩ���뿴������־
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
                    // �Զ���һ��һ����������ʹ��Ӧ����ܷ���Ϊָ����ʽ������������ֻ�����ؿ������е��쳣��
                    options.Filters.Add(new CustomerExceptionFilter());
                });
                // ȫ����Ȩ��Ҫ��ÿ���������µķ�����Ҫ����token��֤�������رգ���������� [AllowAnonymous] ���ɡ�
                builder.Services.AddRazorPages().AddMvcOptions(option =>
                {
                    option.Filters.Add(new AuthorizeFilter());
                });
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                //ע�����
                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(options =>
                {
                    // CustomerExceptionFilter ֻ�����ؿ������е��쳣����� JWT �ٶ�������Զ�����֤ʧ�ܵ���Ӧ
                    JwtBearerConfig.Configure(options);
                });

                // Swagger����ע��������
                builder.Services.AddSwaggerGen(options =>
                {
                    SwaggerConfig.Configure(options);
                });
                // ע��AutoMapper
                builder.Services.AddAutoMapper(cfg =>
                {
                    AutoMapperConfig.Configure(cfg);
                });
                // ����Ĭ�ϵ�����������
                builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterModule(new AutofacConfig());
                });

                // ���ÿ���
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                    {
                        builder.AllowAnyOrigin() // ��������Origin����

                               // �����������󷽷���Get,Post,Put,Delete
                               .AllowAnyMethod()

                               // ������������ͷ:application/json
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
                // �������п���cors����ConfigureServices���������õĿ����������
                app.UseCors("CorsPolicy");
                app.UseHttpsRedirection();
                // ��֤
                app.UseAuthentication();
                // ��Ȩ
                app.UseAuthorization();

                app.MapControllers();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "������ֹ");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}