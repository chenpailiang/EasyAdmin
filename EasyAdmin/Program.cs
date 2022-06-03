using EasyAdmin.Utilitys;
using EasyCommon;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddJwtAuthSetup(builder.Configuration);

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
        var errors = context.ModelState.Values.SelectMany(x => x.Errors.SelectMany(y => y.ErrorMessage));
        var result = new
        {
            Message = "��������",
            Errors = errors
        };
        return new BadRequestObjectResult(result);
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
    s.OperationFilter<ActHeaderFilter>();
    s.OrderActionsBy(x => x.RelativePath);
    var basePath = AppDomain.CurrentDomain.BaseDirectory;
    s.IncludeXmlComments($"{basePath}apicomment.xml", true);
});

// automapperע��
builder.Services.AddAutoMapper();

// servicesע��
foreach (var item in typeof(EasyService.BaseService).Assembly.GetTypes().Where(x => x.BaseType == typeof(EasyService.BaseService)))
{
    builder.Services.AddScoped(item);
}

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

app.UseMiddleware<HttpContextMiddleware>();

app.Run();
