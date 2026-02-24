using Altairis.Application.DTOs;
using Altairis.Application.Services;
using Altairis.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ReservationsController : ControllerBase
{
    private readonly ReservationService _service;

    public ReservationsController(ReservationService service)
    {
        _service = service;
    }

    /// <summary>List reservations with optional filters and pagination.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<ReservationDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResult<ReservationDto>>> GetAll(
        [FromQuery] int? hotelId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] ReservationStatus? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetFilteredAsync(hotelId, from, to, status, page, pageSize);
        return Ok(result);
    }

    /// <summary>Get a single reservation by ID.</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> GetById(int id)
    {
        var reservation = await _service.GetByIdAsync(id);
        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }

    /// <summary>Create a new reservation.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<ReservationDto>> Create(CreateReservationDto dto)
    {
        var reservation = await _service.CreateAsync(dto);
        return Created($"api/reservations/{reservation.Id}", reservation);
    }

    /// <summary>Update reservation status (confirm or cancel).</summary>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationDto>> UpdateStatus(int id, UpdateReservationStatusDto dto)
    {
        var reservation = await _service.UpdateStatusAsync(id, dto);
        if (reservation == null)
            return NotFound();

        return Ok(reservation);
    }
}
