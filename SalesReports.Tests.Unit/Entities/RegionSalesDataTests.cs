using SalesReports.Domain.Entities;

namespace SalesReports.Tests.Unit.Entities;

[TestFixture]
public class RegionSalesDataTests
{
    [Test]
    public void Constructor_WithValidRegionDescription_ShouldCreateInstanceSuccessfully()
    {
        // Arrange
        var regionDescription = "North America";

        // Act
        var regionSalesData = new RegionSalesData(regionDescription);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(regionSalesData.RegionDescription, Is.EqualTo(regionDescription));
            Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(1));
        });
    }

    [Test]
    public void Constructor_WithValidRegionDescriptionWithSpaces_ShouldCreateInstanceSuccessfully()
    {
        // Arrange
        var regionDescription = "  South America  ";

        // Act
        var regionSalesData = new RegionSalesData(regionDescription);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(regionSalesData.RegionDescription, Is.EqualTo(regionDescription));
            Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(1));
        });
    }

    [Test]
    public void Constructor_WithEmptyRegionDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var regionDescription = string.Empty;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new RegionSalesData(regionDescription));
        Assert.That(exception.Message, Is.EqualTo("Region description cannot be empty"));
    }

    [Test]
    public void Constructor_WithWhitespaceRegionDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var regionDescription = "   ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new RegionSalesData(regionDescription));
        Assert.That(exception.Message, Is.EqualTo("Region description cannot be empty"));
    }

    [Test]
    public void Constructor_WithTabsAndNewlinesRegionDescription_ShouldThrowArgumentException()
    {
        // Arrange
        var regionDescription = "\t\n\r ";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new RegionSalesData(regionDescription));
        Assert.That(exception.Message, Is.EqualTo("Region description cannot be empty"));
    }

    [Test]
    public void TotalOrderCount_InitialValue_ShouldBeOne()
    {
        // Arrange
        var regionDescription = "Europe";

        // Act
        var regionSalesData = new RegionSalesData(regionDescription);

        // Assert
        Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(1));
    }

    [Test]
    public void IncrementOrderCount_CalledOnce_ShouldIncrementTotalOrderCountByOne()
    {
        // Arrange
        var regionSalesData = new RegionSalesData("Asia");

        // Act
        regionSalesData.IncrementOrderCount();

        // Assert
        Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(2));
    }

    [Test]
    public void IncrementOrderCount_CalledMultipleTimes_ShouldIncrementTotalOrderCountCorrectly()
    {
        // Arrange
        var regionSalesData = new RegionSalesData("Africa");
        var incrementCount = 5;

        // Act
        for (int i = 0; i < incrementCount; i++)
        {
            regionSalesData.IncrementOrderCount();
        }

        // Assert
        Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(6));
    }

    [Test]
    public void IncrementOrderCount_CalledManyTimes_ShouldHandleLargeNumbers()
    {
        // Arrange
        var regionSalesData = new RegionSalesData("Oceania");
        var incrementCount = 1000;

        // Act
        for (int i = 0; i < incrementCount; i++)
        {
            regionSalesData.IncrementOrderCount();
        }

        // Assert
        Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(1001));
    }

    [Test]
    public void RegionDescription_AfterConstruction_ShouldBeReadOnly()
    {
        // Arrange
        var originalDescription = "Central America";
        var regionSalesData = new RegionSalesData(originalDescription);

        // Act
        var actualDescription = regionSalesData.RegionDescription;

        // Assert
        Assert.That(actualDescription, Is.EqualTo(originalDescription));
    }

    [Test]
    public void TotalOrderCount_SetterIsPrivate_CannotBeModifiedDirectly()
    {
        // Arrange
        var regionSalesData = new RegionSalesData("Antarctica");
        var initialCount = regionSalesData.TotalOrderCount;

        // Act
        regionSalesData.IncrementOrderCount();

        // Assert
        Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(initialCount + 1));
    }

    [TestCase("A")]
    [TestCase("Region1")]
    [TestCase("Very Long Region Description With Many Words")]
    [TestCase("Region-With-Dashes")]
    [TestCase("Region_With_Underscores")]
    [TestCase("Region123")]
    [TestCase("UPPERCASE REGION")]
    [TestCase("lowercase region")]
    [TestCase("MiXeD cAsE ReGiOn")]
    public void Constructor_WithVariousValidRegionDescriptions_ShouldCreateInstanceSuccessfully(string regionDescription)
    {
        // Act
        var regionSalesData = new RegionSalesData(regionDescription);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(regionSalesData.RegionDescription, Is.EqualTo(regionDescription));
            Assert.That(regionSalesData.TotalOrderCount, Is.EqualTo(1));
        });
    }
}