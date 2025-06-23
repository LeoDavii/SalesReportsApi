using Microsoft.AspNetCore.Http;
using SalesReports.Domain.Entities;

namespace SalesReports.App.Services;
public interface ICsvParserService
{
    SalesReport ParseCsvToSalesReport(IFormFile file);
}