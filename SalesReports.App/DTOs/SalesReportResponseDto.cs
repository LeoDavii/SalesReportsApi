namespace SalesReports.App.DTOs;
public record SalesReportResponseDto
{
    public string MostCommonRegion { get; set; } = string.Empty;
    public DateTime FirstOrder { get; set; }
    public DateTime LastOrder { get; set; }
    public int DaysBetweenOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal MedianUnitCost { get; set; }
}
