using Altairis.Domain.Entities;

namespace Altairis.Application.Interfaces;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId);
    Task<RoomType?> GetByIdAsync(int id);
    Task<RoomType> CreateAsync(RoomType roomType);
    Task<int> GetTotalRoomsAsync();
    Task<IEnumerable<RoomType>> GetAllWithHotelAsync();
}
