using Microsoft.AspNetCore.Http;
using SalesReports.App.Model;

namespace SalesReports.App.Services;
public interface ICsvParserService
{
    IEnumerable<SaleRecordModel> ParseToSalesRecords(IFormFile file);
}