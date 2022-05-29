using AdminAuthCenter.Utils;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMysqlSetup(builder.Configuration);
builder.Services.AddJwtSecretSetUp();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("easybuy", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    });
});
builder.Services.AddControllers(x =>
{
    x.Filters.Add<ExceptionFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("easybuy");
app.UseAuthorization();

app.MapControllers();

app.Run();