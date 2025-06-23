using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalesReports.App.DTOs;
using SalesReports.App.Services;
using SalesReports.Domain.Entities;

namespace SalesReports.App.Handlers;
public class SalesReportHandler(ICsvFileValidatorService csvFileValidator,
                                ICsvParserService csvParserService,
                                ILogger<SalesReportHandler> logger) : ISalesReportHandler
{
    public SalesReportResponseDto Handle(IFormFile file)
    {
        logger.LogInformation("Generating report...");

        csvFileValidator.ValidateAndThrowCsvFile(file);
        var salesReport = csvParserService.ParseCsvToSalesReport(file);

        return MapReportToDto(salesReport);
    }

    private static SalesReportResponseDto MapReportToDto(SalesReport salesReport)
    {
        return new()
        {
            DaysBetweenOrders = salesReport.CalculateDaysBetweenOrders(),
            FirstOrder = salesReport.FirstOrderDate,
            LastOrder = salesReport.LastOrderDate,
            MedianUnitCost = salesReport.MedianUnitCost,
            MostCommonRegion = salesReport.GetMostCommonRegion(),
            TotalRevenue = salesReport.TotalRevenue,
        };
    }
}
