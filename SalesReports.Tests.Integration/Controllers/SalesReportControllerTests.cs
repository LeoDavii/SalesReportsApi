using SalesReports.App.DTOs;
using SalesReports.Tests.Integration.Configurations;
using System.Net;
using System.Text;
using System.Text.Json;

namespace SalesReports.Tests.Integration.Controllers;

[TestFixture]
public class SalesReportControllerTests : BaseTest
{
    private const string ApiEndpoint = "/SalesReport";

    [Test]
    public async Task GetSalesReport_WithValidCsvFile_ShouldReturnOkWithReportData()
    {
        // Arrange
        var csvContent = SalesReportControllerTestsHelpers.CreateValidCsvContent();
        var formData = CreateMultipartFormData("sales_data.csv", csvContent);

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseContent = await response.Content.ReadAsStringAsync();
        var salesReport = JsonSerializer.Deserialize<SalesReportResponseDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(salesReport, Is.Not.Null);
    }

    [Test]
    public async Task GetSalesReport_WithInvalidCsvFormat_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidCsvContent = "Invalid,CSV,Format\nWithout,Proper,Headers";
        var formData = CreateMultipartFormData("invalid_data.csv", invalidCsvContent);

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetSalesReport_WithEmptyFile_ShouldReturnBadRequest()
    {
        // Arrange
        var emptyContent = string.Empty;
        var formData = CreateMultipartFormData("empty_file.csv", emptyContent);

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetSalesReport_WithNonCsvFile_ShouldReturnBadRequest()
    {
        // Arrange
        var textContent = "This is just a text file, not a CSV";
        var formData = CreateMultipartFormData("document.txt", textContent, "text/plain");

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetSalesReport_WithMalformedCsvData_ShouldReturnBadRequest()
    {
        // Arrange
        var malformedCsvContent = SalesReportControllerTestsHelpers.CreateMalformedCsvContent();
        var formData = CreateMultipartFormData("malformed_data.csv", malformedCsvContent);

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetSalesReport_WithLargeCsvFile_ShouldReturnOkWithReportData()
    {
        // Arrange
        var largeCsvContent = CreateLargeCsvContent();
        var formData = CreateMultipartFormData("large_sales_data.csv", largeCsvContent);

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var responseContent = await response.Content.ReadAsStringAsync();
        var salesReport = JsonSerializer.Deserialize<SalesReportResponseDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.That(salesReport, Is.Not.Null);
    }

    [Test]
    public async Task GetSalesReport_WithNoFile_ShouldReturnBadRequest()
    {
        // Arrange
        var formData = new MultipartFormDataContent();

        // Act
        var response = await Client.PostAsync(ApiEndpoint, formData);

        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    private static string CreateLargeCsvContent()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit");

        var regions = new[] { "North America", "Europe", "Asia", "South America", "Africa" };
        var countries = new[] { "USA", "Germany", "Japan", "Brazil", "Egypt" };
        var itemTypes = new[] { "Electronics", "Beverages", "Snacks", "Clothing", "Books" };
        var channels = new[] { "Online", "Offline" };
        var priorities = new[] { "H", "M", "L", "C" };

        for (int i = 0; i < 1000; i++)
        {
            var region = regions[i % regions.Length];
            var country = countries[i % countries.Length];
            var itemType = itemTypes[i % itemTypes.Length];
            var channel = channels[i % channels.Length];
            var priority = priorities[i % priorities.Length];
            var orderDate = $"{(i % 12) + 1}/{(i % 28) + 1}/2023";
            var orderId = 100000000 + i;
            var shipDate = $"{(i % 12) + 1}/{(i % 28) + 1}/2023";
            var unitsSold = (i % 1000) + 1;
            var unitPrice = Math.Round(50.0 + (i % 200), 2);
            var unitCost = Math.Round(unitPrice * 0.6, 2);
            var totalRevenue = Math.Round(unitsSold * unitPrice, 2);
            var totalCost = Math.Round(unitsSold * unitCost, 2);
            var totalProfit = Math.Round(totalRevenue - totalCost, 2);

            sb.AppendLine($"{region},{country},{itemType},{channel},{priority},{orderDate},{orderId},{shipDate},{unitsSold},{unitPrice},{unitCost},{totalRevenue},{totalCost},{totalProfit}");
        }

        return sb.ToString();
    }

    private static MultipartFormDataContent CreateMultipartFormData(string fileName, string content, string contentType = "text/csv")
    {
        var formData = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes(content));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
        formData.Add(fileContent, "file", fileName);
        return formData;
    }
}