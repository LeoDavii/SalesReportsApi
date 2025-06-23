namespace SalesReports.Domain.Entities;
public class RegionSalesData
{
    public string RegionDescription { get; }
    public int TotalOrderCount { get; private set; } = 1;

    public RegionSalesData(string regionDescription)
    {
        RegionDescription = !string.IsNullOrWhiteSpace(regionDescription)
            ? regionDescription
            : throw new ArgumentException("Region description cannot be empty");
    }

    public void IncrementOrderCount() => TotalOrderCount++;
}
