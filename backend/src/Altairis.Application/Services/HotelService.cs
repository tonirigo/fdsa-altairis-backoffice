using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;

namespace Altairis.Application.Services;

public class HotelService
{
    private readonly IHotelRepository _repository;

    public HotelService(IHotelRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<HotelDto>> GetAllAsync(int page, int pageSize)
    {
        var (hotels, totalCount) = await _repository.GetPagedAsync(page, pageSize);
        return new PagedResult<HotelDto>
        {
            Items = hotels.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<HotelDto?> GetByIdAsync(int id)
    {
        var hotel = await _repository.GetByIdAsync(id);
        return hotel is null ? null : MapToDto(hotel);
    }

    public async Task<PagedResult<HotelDto>> SearchAsync(string query, int page, int pageSize)
    {
        var (hotels, totalCount) = await _repository.SearchPagedAsync(query, page, pageSize);
        return new PagedResult<HotelDto>
        {
            Items = hotels.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<HotelDto> CreateAsync(CreateHotelDto dto)
    {
        var hotel = new Hotel
        {
            Name = dto.Name,
            Country = dto.Country,
            City = dto.City,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateAsync(hotel);
        return MapToDto(created);
    }

    private static HotelDto MapToDto(Hotel hotel) => new()
    {
        Id = hotel.Id,
        Name = hotel.Name,
        Country = hotel.Country,
        City = hotel.City,
        Address = hotel.Address,
        CreatedAt = hotel.CreatedAt
    };
}
