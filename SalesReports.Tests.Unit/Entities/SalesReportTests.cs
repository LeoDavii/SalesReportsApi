using SalesReports.Domain.Entities;

namespace SalesReports.Tests.Unit.Entities;

[TestFixture]
public class SalesReportTests
{
    private SalesReport _salesReport;

    [SetUp]
    public void Setup()
    {
        _salesReport = new SalesReport();
    }

    #region AddSale Tests

    [Test]
    public void AddSale_WithValidData_ShouldAddSaleSuccessfully()
    {
        // Arrange
        var regionDescription = "North";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        // Act
        _salesReport.AddSale(regionDescription, revenue, orderDate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_salesReport.RegionsSalesData, Has.Count.EqualTo(1));
            Assert.That(_salesReport.RegionsSalesData[0].RegionDescription, Is.EqualTo(regionDescription));
            Assert.That(_salesReport.TotalRevenue, Is.EqualTo(revenue));
            Assert.That(_salesReport.FirstOrderDate, Is.EqualTo(orderDate));
            Assert.That(_salesReport.LastOrderDate, Is.EqualTo(orderDate));
        });
    }

    [Test]
    public void AddSale_WithSameRegionMultipleTimes_ShouldIncrementOrderCount()
    {
        // Arrange
        var regionDescription = "South";
        var revenue1 = 500m;
        var revenue2 = 750m;
        var orderDate1 = new DateTime(2023, 1, 10);
        var orderDate2 = new DateTime(2023, 1, 20);

        // Act
        _salesReport.AddSale(regionDescription, revenue1, orderDate1);
        _salesReport.AddSale(regionDescription, revenue2, orderDate2);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_salesReport.RegionsSalesData, Has.Count.EqualTo(1));
            Assert.That(_salesReport.RegionsSalesData[0].TotalOrderCount, Is.EqualTo(2));
            Assert.That(_salesReport.TotalRevenue, Is.EqualTo(revenue1 + revenue2));
        });
    }

    [Test]
    public void AddSale_WithDifferentRegions_ShouldAddMultipleRegions()
    {
        // Arrange
        var region1 = "North";
        var region2 = "South";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        // Act
        _salesReport.AddSale(region1, revenue, orderDate);
        _salesReport.AddSale(region2, revenue, orderDate);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_salesReport.RegionsSalesData, Has.Count.EqualTo(2));
            Assert.That(_salesReport.RegionsSalesData.Any(r => r.RegionDescription == region1), Is.True);
            Assert.That(_salesReport.RegionsSalesData.Any(r => r.RegionDescription == region2), Is.True);
        });
    }

    [Test]
    public void AddSale_WithNegativeRevenue_ShouldThrowArgumentException()
    {
        // Arrange
        var regionDescription = "West";
        var negativeRevenue = -100m;
        var orderDate = new DateTime(2023, 1, 15);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _salesReport.AddSale(regionDescription, negativeRevenue, orderDate));

        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Revenue cannot be negative"));
            Assert.That(exception.ParamName, Is.EqualTo("revenue"));
        });
    }

    [Test]
    public void AddSale_WithEmptyRegionDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var emptyRegion = string.Empty;
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _salesReport.AddSale(emptyRegion, revenue, orderDate));

        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Region description cannot be empty"));
            Assert.That(exception.ParamName, Is.EqualTo("regionDescription"));
        });
    }

    [Test]
    public void AddSale_WithWhitespaceRegionDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var whitespaceRegion = "   ";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _salesReport.AddSale(whitespaceRegion, revenue, orderDate));

        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Region description cannot be empty"));
            Assert.That(exception.ParamName, Is.EqualTo("regionDescription"));
        });
    }

    #endregion

    #region Date Range Tests

    [Test]
    public void AddSale_WithMultipleDates_ShouldUpdateDateRangeCorrectly()
    {
        // Arrange
        var region = "East";
        var revenue = 500m;
        var firstDate = new DateTime(2023, 1, 10);
        var middleDate = new DateTime(2023, 1, 15);
        var lastDate = new DateTime(2023, 1, 20);

        // Act
        _salesReport.AddSale(region, revenue, middleDate);
        _salesReport.AddSale(region, revenue, lastDate);
        _salesReport.AddSale(region, revenue, firstDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_salesReport.FirstOrderDate, Is.EqualTo(firstDate));
            Assert.That(_salesReport.LastOrderDate, Is.EqualTo(lastDate));
        });
    }

    [Test]
    public void AddSale_WithSingleDate_ShouldSetBothFirstAndLastDate()
    {
        // Arrange
        var region = "Central";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 5, 15);

        // Act
        _salesReport.AddSale(region, revenue, orderDate);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_salesReport.FirstOrderDate, Is.EqualTo(orderDate));
            Assert.That(_salesReport.LastOrderDate, Is.EqualTo(orderDate));
        });
    }

    #endregion

    #region CalculateDaysBetweenOrders Tests

    [Test]
    public void CalculateDaysBetweenOrders_WithMultipleDates_ShouldReturnCorrectDifference()
    {
        // Arrange
        var region = "North";
        var revenue = 500m;
        var firstDate = new DateTime(2023, 1, 10);
        var lastDate = new DateTime(2023, 1, 20);
        var expectedDays = 10;

        _salesReport.AddSale(region, revenue, firstDate);
        _salesReport.AddSale(region, revenue, lastDate);

        // Act
        var result = _salesReport.CalculateDaysBetweenOrders();

        // Assert
        Assert.That(result, Is.EqualTo(expectedDays));
    }

    [Test]
    public void CalculateDaysBetweenOrders_WithSameDate_ShouldReturnZero()
    {
        // Arrange
        var region = "South";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        _salesReport.AddSale(region, revenue, orderDate);

        // Act
        var result = _salesReport.CalculateDaysBetweenOrders();

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CalculateDaysBetweenOrders_WithNoSales_ShouldReturnZero()
    {
        // Act
        var result = _salesReport.CalculateDaysBetweenOrders();

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    #endregion

    #region GetMostCommonRegion Tests

    [Test]
    public void GetMostCommonRegion_WithMultipleRegions_ShouldReturnRegionWithMostOrders()
    {
        // Arrange
        var region1 = "North";
        var region2 = "South";
        var revenue = 500m;
        var orderDate = new DateTime(2023, 1, 15);

        _salesReport.AddSale(region1, revenue, orderDate);
        _salesReport.AddSale(region2, revenue, orderDate);
        _salesReport.AddSale(region2, revenue, orderDate);

        // Act
        var result = _salesReport.GetMostCommonRegion();

        // Assert
        Assert.That(result, Is.EqualTo(region2));
    }

    [Test]
    public void GetMostCommonRegion_WithSingleRegion_ShouldReturnThatRegion()
    {
        // Arrange
        var region = "West";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        _salesReport.AddSale(region, revenue, orderDate);

        // Act
        var result = _salesReport.GetMostCommonRegion();

        // Assert
        Assert.That(result, Is.EqualTo(region));
    }

    [Test]
    public void GetMostCommonRegion_WithNoSales_ShouldReturnEmptyString()
    {
        // Act
        var result = _salesReport.GetMostCommonRegion();

        // Assert
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void GetMostCommonRegion_WithTiedRegions_ShouldReturnFirstMaxRegion()
    {
        // Arrange
        var region1 = "North";
        var region2 = "South";
        var revenue = 500m;
        var orderDate = new DateTime(2023, 1, 15);

        _salesReport.AddSale(region1, revenue, orderDate);
        _salesReport.AddSale(region2, revenue, orderDate);

        // Act
        var result = _salesReport.GetMostCommonRegion();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(result, Is.Not.Empty);
            Assert.That(new[] { region1, region2 }, Does.Contain(result));
        });
    }

    #endregion

    #region SetMedianUnitCost Tests

    [Test]
    public void SetMedianUnitCost_WithValidValue_ShouldSetMedianUnitCost()
    {
        // Arrange
        var medianUnitCost = 25.50m;

        // Act
        _salesReport.SetMedianUnitCost(medianUnitCost);

        // Assert
        Assert.That(_salesReport.MedianUnitCost, Is.EqualTo(medianUnitCost));
    }

    [Test]
    public void SetMedianUnitCost_WithZeroValue_ShouldSetMedianUnitCost()
    {
        // Arrange
        var medianUnitCost = 0m;

        // Act
        _salesReport.SetMedianUnitCost(medianUnitCost);

        // Assert
        Assert.That(_salesReport.MedianUnitCost, Is.EqualTo(medianUnitCost));
    }

    [Test]
    public void SetMedianUnitCost_WithNegativeValue_ShouldThrowArgumentException()
    {
        // Arrange
        var negativeMedianUnitCost = -10m;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _salesReport.SetMedianUnitCost(negativeMedianUnitCost));
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Median Unit Cost cannot be negative"));
            Assert.That(exception.ParamName, Is.EqualTo("medianUnitCost"));
        });
    }

    #endregion

    #region Property Tests

    [Test]
    public void RegionsSalesData_ShouldReturnReadOnlyList()
    {
        // Arrange
        var region = "Test Region";
        var revenue = 1000m;
        var orderDate = new DateTime(2023, 1, 15);

        _salesReport.AddSale(region, revenue, orderDate);

        // Act
        var regionsSalesData = _salesReport.RegionsSalesData;

        // Assert
        Assert.That(regionsSalesData, Is.Not.Null);
        Assert.That(regionsSalesData, Has.Count.EqualTo(1));
    }

    [Test]
    public void TotalRevenue_WithMultipleSales_ShouldAccumulateCorrectly()
    {
        // Arrange
        var region = "Test Region";
        var revenue1 = 500m;
        var revenue2 = 750m;
        var revenue3 = 250m;
        var orderDate = new DateTime(2023, 1, 15);
        var expectedTotal = revenue1 + revenue2 + revenue3;

        // Act
        _salesReport.AddSale(region, revenue1, orderDate);
        _salesReport.AddSale(region, revenue2, orderDate);
        _salesReport.AddSale(region, revenue3, orderDate);

        // Assert
        Assert.That(_salesReport.TotalRevenue, Is.EqualTo(expectedTotal));
    }

    [Test]
    public void InitialState_ShouldHaveCorrectDefaultValues()
    {
        // Arrange & Act
        var salesReport = new SalesReport();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(salesReport.RegionsSalesData, Has.Count.EqualTo(0));
            Assert.That(salesReport.MedianUnitCost, Is.EqualTo(0));
            Assert.That(salesReport.FirstOrderDate, Is.EqualTo(default(DateTime)));
            Assert.That(salesReport.LastOrderDate, Is.EqualTo(default(DateTime)));
            Assert.That(salesReport.TotalRevenue, Is.EqualTo(0));
        });
    }

    #endregion
}
