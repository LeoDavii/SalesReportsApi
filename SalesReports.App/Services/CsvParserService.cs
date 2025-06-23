using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalesReports.App.Model;
using System.Globalization;

namespace SalesReports.App.Services;
public class CsvParserService(ILogger<CsvParserService> logger) : ICsvParserService
{
    private int _recordCount = 0;
    private int _currentRow = 0;

    public IEnumerable<SaleRecordModel> ParseToSalesRecords(IFormFile file)
    {
        try
        {
            var sales = new List<SaleRecordModel>();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                _recordCount++;
                _currentRow = GetSafeRowNumber(csv);

                var salesModel = csv.GetRecord<SaleRecordModel>();
                salesModel.ValidateAndThrow();
            }

            return sales;
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "The provided file is not valid!");
            throw new InvalidDataException($"Error on line: {_currentRow}, {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error has occurred while reading the provided file");
            throw;
        }
    }

    private int GetSafeRowNumber(CsvReader csv)
    {
        try
        {
            return csv.Context?.Parser?.Row ?? _recordCount + 1;
        }
        catch
        {
            return _recordCount + 1;
        }
    }
}
