namespace SalesReports.App.Services;
public interface IMedianCalculatorService
{
    void AddValue(decimal value);
    decimal? GetMedian();
}
