using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using PdfReader.Core.Profiles;

namespace PdfReader.Core.Extensions;

public static class MapperConfigurationExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        var config = new TypeAdapterConfig();
        config.Default.IgnoreNullValues(true);
        TypeAdapterConfig.GlobalSettings.Scan(typeof(PdfDocumentProfiler).Assembly);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}