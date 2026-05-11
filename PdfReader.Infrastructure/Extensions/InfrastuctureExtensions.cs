using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PdfReader.Infrastructure.Context;
using PdfReader.Infrastructure.Services;

namespace PdfReader.Infrastructure.Extensions;

public static class InfrastuctureExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
      
        services.AddTransient<IPdfDocumentService, PdfDocumentService>()
            .AddScoped<IPdfTextExtractor, PdfTextExtractor>()
            .AddDbContext<PdfReaderDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        return services;
    }
    
}