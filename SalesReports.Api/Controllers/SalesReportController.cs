using Microsoft.AspNetCore.Mvc;
using SalesReports.App.DTOs;
using SalesReports.App.Handlers;

namespace SalesReports.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesReportController(ISalesReportHandler salesReportHandler, ILogger<SalesReportController> logger) : ControllerBase
{

    /// <summary>
    ///     Retrieves a sales report generated from the provided file.
    /// </summary>
    /// <returns>Returns a <see cref="SalesReportResponseDto"/> with sales data summary.</returns>
    /// <response code="200">The request was accepted.</response>
    /// <response code="400">The request was unsuccessful, see details.</response>
    /// <response code="500">The request was unsuccessful.</response>
    [HttpPost()]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(SalesReportResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateSalesReportFromFile(IFormFile file)
    {
        try
        {
            return Ok(salesReportHandler.Handle(file));
        }
        catch (ArgumentException ex)
        {
            LogReportError(ex);
            return BadRequest(ex.Message);
        }
        catch (InvalidCastException ex)
        {
            LogReportError(ex);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            LogReportError(ex);
            return StatusCode(500, "Something went wrong while generating sales report.");
        }
    }

    private void LogReportError(Exception ex)
    {
        logger.LogError(ex, "Something went wrong while generating sales report.");
    }
}