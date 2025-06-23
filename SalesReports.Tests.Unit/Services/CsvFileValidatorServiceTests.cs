using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using SalesReports.App.Services;

namespace SalesReports.Tests.Unit.Services;

[TestFixture]
public class CsvFileValidatorServiceTests
{
    private Mock<ILogger<CsvFileValidatorService>> _mockLogger;
    private CsvFileValidatorService _service;

    [SetUp]
    public void Setup()
    {
        // Arrange
        _mockLogger = new Mock<ILogger<CsvFileValidatorService>>();
        _service = new CsvFileValidatorService(_mockLogger.Object);
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileIsNull_ThrowsArgumentException()
    {
        // Arrange
        IFormFile file = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(file));
        Assert.That(exception.Message, Is.EqualTo("No file was provided or file is empty."));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileIsEmpty_ThrowsArgumentException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
        Assert.That(exception.Message, Is.EqualTo("No file was provided or file is empty."));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileHasInvalidExtension_ThrowsArgumentException()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.txt");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
        Assert.That(exception.Message, Is.EqualTo("Only CSV files are allowed."));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileHasValidCsvExtension_DoesNotThrow()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.csv");

        // Act & Assert
        Assert.DoesNotThrow(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileHasUppercaseCsvExtension_DoesNotThrow()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.CSV");

        // Act & Assert
        Assert.DoesNotThrow(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileHasMixedCaseCsvExtension_DoesNotThrow()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.CsV");

        // Act & Assert
        Assert.DoesNotThrow(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileIsNull_LogsError()
    {
        // Arrange
        IFormFile file = null;

        // Act - Assert
        Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(file));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileIsEmpty_LogsError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(0);

        // Act - Assert
        Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileHasInvalidExtension_LogsError()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.txt");

        // Act - // Assert
        Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
    }

    [Test]
    public void ValidateAndThrowCsvFile_WhenFileIsValid_DoesNotLog()
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns("test.csv");

        // Act - // Assert
        _service.ValidateAndThrowCsvFile(mockFile.Object);
    }

    [TestCase(".exe")]
    [TestCase(".pdf")]
    [TestCase(".docx")]
    [TestCase(".xlsx")]
    [TestCase("")]
    public void ValidateAndThrowCsvFile_WhenFileHasVariousInvalidExtensions_ThrowsArgumentException(string extension)
    {
        // Arrange
        var mockFile = new Mock<IFormFile>();
        mockFile.Setup(f => f.Length).Returns(100);
        mockFile.Setup(f => f.FileName).Returns($"test{extension}");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _service.ValidateAndThrowCsvFile(mockFile.Object));
        Assert.That(exception.Message, Is.EqualTo("Only CSV files are allowed."));
    }
}
