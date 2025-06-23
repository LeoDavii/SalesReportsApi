namespace SalesReports.Domain.Services;
public interface IMedianCalculatorService
{
    void AddValue(decimal value);
    decimal? GetMedian();
}
