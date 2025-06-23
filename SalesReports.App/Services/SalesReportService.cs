using SalesReports.App.Model;
using SalesReports.Domain.Entities;
using SalesReports.Domain.Services;

namespace SalesReports.App.Services;
public class SalesReportService(IMedianCalculatorService medianCalculatorService) : ISalesReportService
{
    public SalesReport GetSalesReport(IEnumerable<SaleRecordModel> sales)
    {
        var salesReport = new SalesReport();

        foreach (var sale in sales)
        {
            medianCalculatorService.AddValue(sale.UnitCost);
            salesReport.AddSale(sale.RegionDescription, sale.TotalRevenue, sale.OrderDate);
        }

        var unitCostMedian = medianCalculatorService.GetMedian();

        if (unitCostMedian.HasValue)
        {
            salesReport.SetMedianUnitCost(unitCostMedian.Value);
        }

        return salesReport;
    }
}

