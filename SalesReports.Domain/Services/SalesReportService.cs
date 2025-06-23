using SalesReports.Domain.Entities;

namespace SalesReports.Domain.Services;
public class SalesReportService(IMedianCalculatorService medianCalculatorService) : ISalesReportService
{
    public SalesReport GetSalesReport(IEnumerable<Sale> sales)
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

