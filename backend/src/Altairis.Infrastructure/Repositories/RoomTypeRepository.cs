using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly AltairisDbContext _context;

    public RoomTypeRepository(AltairisDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomType>> GetByHotelIdAsync(int hotelId)
    {
        return await _context.RoomTypes
            .Include(r => r.Category)
            .Where(r => r.HotelId == hotelId)
            .OrderBy(r => r.Name)
            .ToListAsync();
    }

    public async Task<RoomType?> GetByIdAsync(int id)
    {
        return await _context.RoomTypes.FindAsync(id);
    }

    public async Task<RoomType> CreateAsync(RoomType roomType)
    {
        _context.RoomTypes.Add(roomType);
        await _context.SaveChangesAsync();
        return roomType;
    }

    public async Task<int> GetTotalRoomsAsync()
    {
        return await _context.RoomTypes.SumAsync(rt => rt.TotalRooms);
    }

    public async Task<IEnumerable<RoomType>> GetAllWithHotelAsync()
    {
        return await _context.RoomTypes
            .Include(rt => rt.Hotel)
            .ToListAsync();
    }
}
