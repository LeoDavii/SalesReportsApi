using CsvHelper.Configuration.Attributes;

namespace SalesReports.App.Model;
public record SaleRecordModel
{
    [Name("Region")]
    public string RegionDescription { get; private set; }
    
    [Name("Unit Cost")]
    public decimal UnitCost { get; private set; }
    
    [Name("Order Date")]
    public DateTime OrderDate { get; private set; }
    
    [Name("Total Revenue")]
    public decimal TotalRevenue { get; private set; }

    public SaleRecordModel(string regionDescription, decimal unitCost, DateTime orderDate, decimal totalRevenue)
    {
        RegionDescription = regionDescription;
        UnitCost = unitCost;
        OrderDate = orderDate;
        TotalRevenue = totalRevenue;
    }

    public void ValidateAndThrow()
    {
        if (string.IsNullOrWhiteSpace(RegionDescription))
            throw new ArgumentException("Region Description is required");

        if (UnitCost <= 0)
            throw new ArgumentException("Unit Cost must be greater than zero");

        if (OrderDate == default || OrderDate > DateTime.Now)
            throw new ArgumentException("Order Date must be a valid date and cannot be in the future");

        if (TotalRevenue < 0)
            throw new ArgumentException("Total Revenue cannot be negative");
    }
}