using Altairis.Application.DTOs;
using Altairis.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.Api.Controllers;

[ApiController]
[Route("api/room-categories")]
[Produces("application/json")]
public class RoomCategoriesController : ControllerBase
{
    private readonly RoomCategoryService _service;

    public RoomCategoriesController(RoomCategoryService service)
    {
        _service = service;
    }

    /// <summary>List all room categories.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RoomCategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RoomCategoryDto>>> GetAll()
    {
        var categories = await _service.GetAllAsync();
        return Ok(categories);
    }

    /// <summary>Create a new room category.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RoomCategoryDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<RoomCategoryDto>> Create(CreateRoomCategoryDto dto)
    {
        var category = await _service.CreateAsync(dto);
        return Created($"api/room-categories/{category.Id}", category);
    }
}
