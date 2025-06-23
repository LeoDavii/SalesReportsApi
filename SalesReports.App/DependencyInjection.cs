using Microsoft.Extensions.DependencyInjection;
using SalesReports.App.Handlers;
using SalesReports.App.Services;

namespace SalesReports.App;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISalesReportHandler, SalesReportHandler>();
        services.AddScoped<ICsvParserService, CsvParserService>();
        services.AddScoped<IMedianCalculatorService, MedianCalculatorService>();
        services.AddScoped<ICsvFileValidatorService, CsvFileValidatorService>();

        return services;
    }
}
