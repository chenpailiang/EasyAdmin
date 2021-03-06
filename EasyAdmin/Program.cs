using EasyAdmin.Utilitys;
using EasyCommon;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;
using System.Reflection;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    ConfigTool.config = builder.Configuration;
    // Add services to the container.

    builder.Services.AddRouting(op => op.LowercaseUrls = true);
    builder.Services.AddControllers(x =>
    {
        x.Filters.Add<AuthCheckFilter>(); // 鉴权
        x.Filters.Add<GlobalExceptionFilter>(); // 全局异常过滤
    });
    builder.Services.AddEndpointsApiExplorer();

    // 跨域
    builder.Services.AddCors(opt =>
    {
        opt.AddPolicy("easyadmin", builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        });
    });
    builder.Services.AddJwtAuthSetup();

    var currentAssembly = Assembly.GetExecutingAssembly();

    // 注入参数校验器FluentValidation
    builder.Services.AddFluentValidation(cfg =>
    {
        cfg.RegisterValidatorsFromAssembly(currentAssembly);
    });
    builder.Services.Configure<ApiBehaviorOptions>(op =>
    {
        op.InvalidModelStateResponseFactory = (context) =>
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
            {
                var errors = context.ModelState.Values.SelectMany(x => x.Errors);
                var result = new
                {
                    message = "参数错误",
                    errors = errors.Select(x => x.ErrorMessage)
                };
                return new BadRequestObjectResult(result);
            }
            else
            {
                return new BadRequestObjectResult(new { message = "参数错误" });
            }
        };
    });
    var currentAssemblyName = currentAssembly.GetName().Name;
    // swagger配置
    builder.Services.AddSwaggerGen(s =>
    {

        s.SwaggerDoc("easybuy", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Version = "v1",
            Title = $"{currentAssemblyName} Api "
        });
        s.OrderActionsBy(x => x.RelativePath);
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        s.IncludeXmlComments($"{basePath}EasyAdmin.xml", true);
        s.IncludeXmlComments($"{basePath}EasyService.xml", true);
    });

    // automapper注入
    builder.Services.AddAutoMapper();

    // services注入
    foreach (var item in typeof(EasyService.Service.RoleService).Assembly.GetTypes().Where(x => x.Name.EndsWith("Service")))
    {
        builder.Services.AddScoped(item);
    }

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<CurrentUser>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(op => { op.RouteTemplate = "swagger/{documentName}/swagger.json"; });
        app.UseSwaggerUI(op =>
        {
            op.SwaggerEndpoint("/swagger/easybuy/swagger.json", $"{currentAssemblyName} Docs");
            op.RoutePrefix = "swagger";
        });
    }
    app.UseCors("easyadmin");
    app.UseAuthorization();
    app.UseAuthentication();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}