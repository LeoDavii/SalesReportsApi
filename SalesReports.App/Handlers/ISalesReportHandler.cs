using Microsoft.AspNetCore.Http;
using SalesReports.App.DTOs;

namespace SalesReports.App.Handlers;
public interface ISalesReportHandler
{
    SalesReportResponseDto Handle(IFormFile file);
}