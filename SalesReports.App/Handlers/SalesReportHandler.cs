using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalesReports.App.DTOs;
using SalesReports.App.Services;

namespace SalesReports.App.Handlers;
public class SalesReportHandler(ICsvFileValidatorService csvFileValidator, 
                                ICsvParserService csvParserService, 
                                ISalesReportService salesReportService,
                                ILogger<SalesReportHandler> logger)
{
    public SalesReportResponseDto Handle(IFormFile file)
    {
        logger.LogInformation("Generating report...");

        csvFileValidator.ValidateAndThrowCsvFile(file);
        var sales = csvParserService.ParseToSalesRecords(file);

        var salesReport = salesReportService.GetSalesReport(sales);

        return MapReportToDto(salesReport);
    }

    private static SalesReportResponseDto MapReportToDto(Domain.Entities.SalesReport salesReport)
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
