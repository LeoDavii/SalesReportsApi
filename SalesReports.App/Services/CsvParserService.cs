using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SalesReports.App.Model;
using SalesReports.Domain.Entities;
using System.Globalization;

namespace SalesReports.App.Services;
public class CsvParserService(ILogger<CsvParserService> logger, IMedianCalculatorService medianCalculatorService) : ICsvParserService
{
    private int _recordCount = 0;
    private int _currentRow = 0;

    public SalesReport ParseCsvToSalesReport(IFormFile file)
    {
        try
        {
            SalesReport salesReport = new();

            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                CountRow(csv);
                ProcessRecord(salesReport, csv);
            }

            EnrichReportWithMedianUnitCost(salesReport);

            return salesReport;
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

    private void CountRow(CsvReader csv)
    {
        _recordCount++;
        _currentRow = GetSafeRowNumber(csv);
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

    private void ProcessRecord(SalesReport salesReport, CsvReader csv)
    {
        SaleRecordModel salesModel = GetRecord(csv);
        salesReport.AddSale(salesModel.Region, salesModel.TotalRevenue, salesModel.OrderDate);
        medianCalculatorService.AddValue(salesModel.UnitCost);
    }
    private static SaleRecordModel GetRecord(CsvReader csv)
    {
        var salesModel = csv.GetRecord<SaleRecordModel>();
        salesModel.ValidateAndThrow();
        return salesModel;
    }

    private void EnrichReportWithMedianUnitCost(SalesReport salesReport)
    {
        var medianUnitCost = medianCalculatorService.GetMedian();

        if (!medianUnitCost.HasValue)
            throw new ArgumentException("Unable to calculate Median Unit Cost");

        salesReport.SetMedianUnitCost(medianUnitCost.Value);
    }


}
