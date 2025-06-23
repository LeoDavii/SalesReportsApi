using Microsoft.AspNetCore.Mvc;
using SalesReports.App.DTOs;
using SalesReports.App.Handlers;

namespace SalesReports.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesReportController(ISalesReportHandler salesReportHandler, ILogger<SalesReportController> logger) : ControllerBase
{

    /// <summary>
    ///     Get Sales Report
    /// </summary>
    /// <returns>Response Data</returns>
    /// <response code="200">The request was accepted.</response>
    /// <response code="204">The request was accepted, no content.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    [HttpPost()]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SalesReportResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetSalesReport(IFormFile file)
    {
        try
        {
            return Ok(salesReportHandler.Handle(file));
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Something went wrong while generating sales report.");
            return BadRequest(ex.Message);
        }
    }
}