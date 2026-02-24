using Altairis.Application.DTOs;
using Altairis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.Api.Controllers;

[ApiController]
[Produces("application/json")]
public class AvailabilityController : ControllerBase
{
    private readonly AvailabilityService _service;

    public AvailabilityController(AvailabilityService service)
    {
        _service = service;
    }

    /// <summary>Get hotel inventory availability grid for a date range.</summary>
    [HttpGet("api/hotels/{hotelId}/availability")]
    [ProducesResponseType(typeof(HotelInventoryGridDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<HotelInventoryGridDto>> GetHotelGrid(int hotelId, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
    {
        var fromDate = from ?? DateTime.UtcNow.Date;
        var toDate = to ?? fromDate.AddDays(6);
        var grid = await _service.GetHotelGridAsync(hotelId, fromDate, toDate);
        return Ok(grid);
    }
}
