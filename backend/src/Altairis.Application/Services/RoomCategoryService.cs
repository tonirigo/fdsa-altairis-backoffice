using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;

namespace Altairis.Application.Services;

public class RoomCategoryService
{
    private readonly IRoomCategoryRepository _repository;

    public RoomCategoryService(IRoomCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoomCategoryDto>> GetAllAsync()
    {
        var categories = await _repository.GetAllAsync();
        return categories.Select(c => new RoomCategoryDto { Id = c.Id, Name = c.Name });
    }

    public async Task<RoomCategoryDto> CreateAsync(CreateRoomCategoryDto dto)
    {
        var category = new RoomCategory { Name = dto.Name };
        var created = await _repository.CreateAsync(category);
        return new RoomCategoryDto { Id = created.Id, Name = created.Name };
    }
}
