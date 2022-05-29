using Microsoft.Extensions.DependencyInjection;

namespace EasyService.Mapper;

public static class MapperConfig
{
    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MapperConfig).Assembly);
    }
}
