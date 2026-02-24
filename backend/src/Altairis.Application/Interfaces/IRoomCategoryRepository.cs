using Altairis.Domain.Entities;

namespace Altairis.Application.Interfaces;

public interface IRoomCategoryRepository
{
    Task<IEnumerable<RoomCategory>> GetAllAsync();
    Task<RoomCategory> CreateAsync(RoomCategory category);
}
