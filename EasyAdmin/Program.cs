using EasyAdmin.Utilitys;
using EasyCommon;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
ConfigTool.config = builder.Configuration;
// Add services to the container.

builder.Services.AddRouting(op => op.LowercaseUrls = true);
builder.Services.AddControllers(x =>
{
    x.Filters.Add<AuthCheckFilter>(); // ��Ȩ
    x.Filters.Add<GlobalExceptionFilter>(); // ȫ���쳣����
});
builder.Services.AddEndpointsApiExplorer();

// ����
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("easybuy", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});
builder.Services.AddJwtAuthSetup();

var currentAssembly = Assembly.GetExecutingAssembly();

// ע�����У����FluentValidation
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
                message = "��������",
                errors = errors.Select(x => x.ErrorMessage)
            };
            return new BadRequestObjectResult(result);
        }
        else
        {
            return new BadRequestObjectResult(new { message = "��������" });
        }
    };
});
var currentAssemblyName = currentAssembly.GetName().Name;
// swagger����
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

// automapperע��
builder.Services.AddAutoMapper();

// servicesע��
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
app.UseCors("easybuy");
app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
