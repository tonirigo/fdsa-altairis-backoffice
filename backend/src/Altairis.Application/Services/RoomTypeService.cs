using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;

namespace Altairis.Application.Services;

public class RoomTypeService
{
    private readonly IRoomTypeRepository _repository;

    public RoomTypeService(IRoomTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoomTypeDto>> GetByHotelIdAsync(int hotelId)
    {
        var roomTypes = await _repository.GetByHotelIdAsync(hotelId);
        return roomTypes.Select(MapToDto);
    }

    public async Task<RoomTypeDto> CreateAsync(int hotelId, CreateRoomTypeDto dto)
    {
        var roomType = new RoomType
        {
            HotelId = hotelId,
            CategoryId = dto.CategoryId,
            Name = dto.Name,
            Capacity = dto.Capacity,
            TotalRooms = dto.TotalRooms
        };

        var created = await _repository.CreateAsync(roomType);
        return MapToDto(created);
    }

    private static RoomTypeDto MapToDto(RoomType roomType) => new()
    {
        Id = roomType.Id,
        HotelId = roomType.HotelId,
        CategoryId = roomType.CategoryId,
        CategoryName = roomType.Category?.Name,
        Name = roomType.Name,
        Capacity = roomType.Capacity,
        TotalRooms = roomType.TotalRooms
    };
}
