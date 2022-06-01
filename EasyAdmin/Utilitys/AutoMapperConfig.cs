using AutoMapper;
using EasyService.Mapper;

namespace EasyAdmin.Utilitys;

public static class AutoMapperConfig
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.ClearPrefixes();
            cfg.AddMaps(typeof(SystemManageProfile).Assembly);
        });
        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
}