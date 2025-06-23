namespace SalesReports.Domain.Entities;
public class SalesReport
{
    private readonly List<RegionSalesData> _regionRecords = [];

    public IReadOnlyList<RegionSalesData> RegionsSalesData => _regionRecords.AsReadOnly();

    public decimal MedianUnitCost { get; private set; }
    public DateTime FirstOrderDate { get; private set; }
    public DateTime LastOrderDate { get; private set; }
    public decimal TotalRevenue { get; private set; }

    public void AddSale(string regionDescription, decimal revenue, DateTime orderDate)
    {
        UpsertRegion(regionDescription);
        IncreaseTotalRevenue(revenue);
        UpdateDateRange(orderDate);
    }

    private void IncreaseTotalRevenue(decimal revenue)
    {
        if (revenue < 0)
            throw new ArgumentException("Revenue cannot be negative.", nameof(revenue));

        TotalRevenue += revenue;
    }

    private void UpsertRegion(string regionDescription)
    {
        if (string.IsNullOrWhiteSpace(regionDescription))
            throw new ArgumentException("Region description cannot be empty.", nameof(regionDescription));

        var regionRecord = _regionRecords.FirstOrDefault(r => r.RegionDescription == regionDescription);

        if (regionRecord == null)
        {
            _regionRecords.Add(new RegionSalesData(regionDescription));
            return;
        }

        regionRecord.IncrementOrderCount();
    }

    private void UpdateDateRange(DateTime orderDate)
    {
        if (orderDate == default || orderDate > DateTime.Now)
            throw new ArgumentException("Order Date must be a valid date and cannot be in the future");

        if (FirstOrderDate == default || orderDate < FirstOrderDate)
            FirstOrderDate = orderDate;

        if (LastOrderDate == default || orderDate > LastOrderDate)
            LastOrderDate = orderDate;
    }

    public int CalculateDaysBetweenOrders() => (LastOrderDate - FirstOrderDate).Days;

    public string GetMostCommonRegion() =>
        RegionsSalesData.MaxBy(r => r.TotalOrderCount)?.RegionDescription ?? string.Empty;

    public void SetMedianUnitCost(decimal medianUnitCost)
    {
        if (medianUnitCost < 0)
            throw new ArgumentException("Median Unit Cost cannot be negative.", nameof(medianUnitCost));

        MedianUnitCost = medianUnitCost;
    }
}

