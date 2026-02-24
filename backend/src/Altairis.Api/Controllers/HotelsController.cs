using Altairis.Application.DTOs;
using Altairis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class HotelsController : ControllerBase
{
    private readonly HotelService _service;

    public HotelsController(HotelService service)
    {
        _service = service;
    }

    /// <summary>List hotels with pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<HotelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<HotelDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetAllAsync(page, pageSize);
        return Ok(result);
    }

    /// <summary>Get a single hotel by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HotelDto>> GetById(int id)
    {
        var hotel = await _service.GetByIdAsync(id);
        if (hotel is null) return NotFound();
        return Ok(hotel);
    }

    /// <summary>Search hotels by name, city or country.</summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<HotelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<HotelDto>>> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _service.SearchAsync(query, page, pageSize);
        return Ok(result);
    }

    /// <summary>Create a new hotel.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(HotelDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<HotelDto>> Create(CreateHotelDto dto)
    {
        var hotel = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = hotel.Id }, hotel);
    }
}
