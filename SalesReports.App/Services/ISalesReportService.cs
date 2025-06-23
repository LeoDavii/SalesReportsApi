using SalesReports.App.Model;
using SalesReports.Domain.Entities;

namespace SalesReports.App.Services;
public interface ISalesReportService
{
    SalesReport GetSalesReport(IEnumerable<SaleRecordModel> sales);
}