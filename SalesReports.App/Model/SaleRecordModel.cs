using CsvHelper.Configuration.Attributes;

namespace SalesReports.App.Model;
public class SaleRecordModel
{
    [Name("Region")]
    public string Region { get; set; } = string.Empty;
    
    [Name("Unit Cost")]
    public decimal UnitCost { get; set; }
    
    [Name("Order Date")]
    public DateTime OrderDate { get; set; }
    
    [Name("Total Revenue")]
    public decimal TotalRevenue { get; set; }

    public void ValidateAndThrow()
    {
        if (string.IsNullOrWhiteSpace(Region))
            throw new ArgumentException("Region Description is required");

        if (UnitCost <= 0)
            throw new ArgumentException("Unit Cost must be greater than zero");

        if (OrderDate == default || OrderDate > DateTime.Now)
            throw new ArgumentException("Order Date must be a valid date and cannot be in the future");

        if (TotalRevenue < 0)
            throw new ArgumentException("Total Revenue cannot be negative");
    }
}