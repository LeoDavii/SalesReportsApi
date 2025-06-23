using SalesReports.App.Services;

namespace SalesReports.Tests.Unit.Services;

[TestFixture]
public class MedianCalculatorServiceTests
{
    private MedianCalculatorService _medianCalculatorService;

    [SetUp]
    public void SetUp()
    {
        // Arrange
        _medianCalculatorService = new MedianCalculatorService();
    }

    [Test]
    public void Constructor_WhenCalled_ShouldInitializeCountToZero()
    {
        // Arrange

        // Act
        var count = _medianCalculatorService.Count;

        // Assert
        Assert.That(count, Is.EqualTo(0));
    }

    [Test]
    public void GetMedian_WhenNoValuesAdded_ShouldReturnNull()
    {
        // Arrange

        // Act
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.Null);
    }

    [Test]
    public void AddValue_WhenFirstValueAdded_ShouldIncrementCount()
    {
        // Arrange
        decimal value = 10.5m;

        // Act
        _medianCalculatorService.AddValue(value);

        // Assert
        Assert.That(_medianCalculatorService.Count, Is.EqualTo(1));
    }

    [Test]
    public void GetMedian_WhenSingleValueAdded_ShouldReturnThatValue()
    {
        // Arrange
        decimal value = 42.7m;

        // Act
        _medianCalculatorService.AddValue(value);
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(value));
    }

    [Test]
    public void GetMedian_WhenTwoValuesAdded_ShouldReturnAverage()
    {
        // Arrange
        decimal value1 = 10m;
        decimal value2 = 20m;
        decimal expectedMedian = 15m;

        // Act
        _medianCalculatorService.AddValue(value1);
        _medianCalculatorService.AddValue(value2);
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenThreeValuesAdded_ShouldReturnMiddleValue()
    {
        // Arrange
        decimal value1 = 30m;
        decimal value2 = 10m;
        decimal value3 = 20m;
        decimal expectedMedian = 20m;

        // Act
        _medianCalculatorService.AddValue(value1);
        _medianCalculatorService.AddValue(value2);
        _medianCalculatorService.AddValue(value3);
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenFourValuesAdded_ShouldReturnAverageOfMiddleTwo()
    {
        // Arrange
        decimal value1 = 10m;
        decimal value2 = 40m;
        decimal value3 = 20m;
        decimal value4 = 30m;
        decimal expectedMedian = 25m;

        // Act
        _medianCalculatorService.AddValue(value1);
        _medianCalculatorService.AddValue(value2);
        _medianCalculatorService.AddValue(value3);
        _medianCalculatorService.AddValue(value4);
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void AddValue_WhenMultipleValuesAdded_ShouldUpdateCountCorrectly()
    {
        // Arrange
        decimal[] values = [5m, 15m, 25m, 35m, 45m];

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }

        // Assert
        Assert.That(_medianCalculatorService.Count, Is.EqualTo(values.Length));
    }

    [Test]
    public void GetMedian_WhenFiveValuesAdded_ShouldReturnMiddleValue()
    {
        // Arrange
        decimal[] values = [50m, 10m, 30m, 40m, 20m];
        decimal expectedMedian = 30m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenNegativeValuesAdded_ShouldHandleCorrectly()
    {
        // Arrange
        decimal[] values = [-10m, -5m, -15m];
        decimal expectedMedian = -10m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenMixedPositiveAndNegativeValues_ShouldHandleCorrectly()
    {
        // Arrange
        decimal[] values = [-10m, 5m, -5m, 10m];
        decimal expectedMedian = 0m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenDuplicateValuesAdded_ShouldHandleCorrectly()
    {
        // Arrange
        decimal[] values = [10m, 10m, 20m, 20m, 30m];
        decimal expectedMedian = 20m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenDecimalPrecisionValues_ShouldHandleCorrectly()
    {
        // Arrange
        decimal[] values = [1.1m, 2.2m, 3.3m, 4.4m];
        decimal expectedMedian = 2.75m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenLargeNumberOfValues_ShouldHandleCorrectly()
    {
        // Arrange
        int numberOfValues = 1000;
        decimal expectedMedian = 500.5m;

        // Act
        for (int i = 1; i <= numberOfValues; i++)
        {
            _medianCalculatorService.AddValue(i);
        }
        var median = _medianCalculatorService.GetMedian();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(median, Is.EqualTo(expectedMedian));
            Assert.That(_medianCalculatorService.Count, Is.EqualTo(numberOfValues));
        });
    }

    [Test]
    public void AddValue_WhenZeroAdded_ShouldHandleCorrectly()
    {
        // Arrange
        decimal[] values = [-1m, 0m, 1m];
        decimal expectedMedian = 0m;

        // Act
        foreach (var value in values)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        // Assert
        Assert.That(median, Is.EqualTo(expectedMedian));
    }

    [Test]
    public void GetMedian_WhenSameValueAddedMultipleTimes_ShouldReturnThatValue()
    {
        // Arrange
        decimal value = 15.5m;
        int timesToAdd = 5;

        // Act
        for (int i = 0; i < timesToAdd; i++)
        {
            _medianCalculatorService.AddValue(value);
        }
        var median = _medianCalculatorService.GetMedian();

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(median, Is.EqualTo(value));
            Assert.That(_medianCalculatorService.Count, Is.EqualTo(timesToAdd));
        });
    }
}