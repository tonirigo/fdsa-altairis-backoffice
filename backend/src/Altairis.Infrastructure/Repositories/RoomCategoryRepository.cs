using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;
using Altairis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Repositories;

public class RoomCategoryRepository : IRoomCategoryRepository
{
    private readonly AltairisDbContext _context;

    public RoomCategoryRepository(AltairisDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomCategory>> GetAllAsync()
    {
        return await _context.RoomCategories.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<RoomCategory> CreateAsync(RoomCategory category)
    {
        _context.RoomCategories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }
}
