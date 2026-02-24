using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Repositories;

public class HotelRepository : IHotelRepository
{
    private readonly AltairisDbContext _context;

    public HotelRepository(AltairisDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Hotel>> GetAllAsync()
    {
        return await _context.Hotels.OrderBy(h => h.Name).ToListAsync();
    }

    public async Task<(IEnumerable<Hotel> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _context.Hotels.OrderBy(h => h.Name);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Hotel?> GetByIdAsync(int id)
    {
        return await _context.Hotels.Include(h => h.RoomTypes).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<IEnumerable<Hotel>> SearchAsync(string query)
    {
        return await _context.Hotels
            .Where(h => h.Name.Contains(query) || h.City.Contains(query) || h.Country.Contains(query))
            .OrderBy(h => h.Name)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Hotel> Items, int TotalCount)> SearchPagedAsync(string searchQuery, int page, int pageSize)
    {
        var query = _context.Hotels
            .Where(h => h.Name.Contains(searchQuery) || h.City.Contains(searchQuery) || h.Country.Contains(searchQuery))
            .OrderBy(h => h.Name);
        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public async Task<Hotel> CreateAsync(Hotel hotel)
    {
        _context.Hotels.Add(hotel);
        await _context.SaveChangesAsync();
        return hotel;
    }
}
