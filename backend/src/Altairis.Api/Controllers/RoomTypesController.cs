using Altairis.Application.DTOs;
using Altairis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.Api.Controllers;

[ApiController]
[Route("api/hotels/{hotelId}/room-types")]
[Produces("application/json")]
public class RoomTypesController : ControllerBase
{
    private readonly RoomTypeService _service;

    public RoomTypesController(RoomTypeService service)
    {
        _service = service;
    }

    /// <summary>List room types for a hotel.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoomTypeDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomTypeDto>>> GetByHotelId(int hotelId)
    {
        var roomTypes = await _service.GetByHotelIdAsync(hotelId);
        return Ok(roomTypes);
    }

    /// <summary>Create a room type for a hotel.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoomTypeDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<RoomTypeDto>> Create(int hotelId, CreateRoomTypeDto dto)
    {
        var roomType = await _service.CreateAsync(hotelId, dto);
        return Created($"api/hotels/{hotelId}/room-types/{roomType.Id}", roomType);
    }
}
