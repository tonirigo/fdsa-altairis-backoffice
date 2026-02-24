using Altairis.Domain.Entities;

namespace Altairis.Application.Interfaces;

public interface IHotelRepository
{
    Task<IEnumerable<Hotel>> GetAllAsync();
    Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
    Task<Hotel?> GetByIdAsync(int id);
    Task<IEnumerable<Hotel>> SearchAsync(string query);
    Task<(IEnumerable<Hotel> Items, int TotalCount)> SearchPagedAsync(string query, int page, int pageSize);
    Task<Hotel> CreateAsync(Hotel hotel);
}
