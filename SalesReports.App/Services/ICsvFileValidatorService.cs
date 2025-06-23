using Microsoft.AspNetCore.Http;

namespace SalesReports.App.Services;
public interface ICsvFileValidatorService
{
    void ValidateAndThrowCsvFile(IFormFile file);
}