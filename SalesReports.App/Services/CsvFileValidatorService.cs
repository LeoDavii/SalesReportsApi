using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SalesReports.App.Services;
public class CsvFileValidatorService(ILogger<CsvFileValidatorService> logger) : ICsvFileValidatorService
{
    public void ValidateAndThrowCsvFile(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file was provided or file is empty.");
            }

            var allowedExtensions = new[] { ".csv" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Only CSV files are allowed.");
            }
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "The provided file is not valid!");
            throw;
        }
    }
}
