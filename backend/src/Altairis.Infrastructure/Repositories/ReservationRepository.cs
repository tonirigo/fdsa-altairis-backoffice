using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;
using Altairis.Domain.Enums;
using Altairis.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AltairisDbContext _context;

    public ReservationRepository(AltairisDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations.OrderByDescending(r => r.CheckIn).ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.RoomType)
            .ThenInclude(rt => rt.Hotel)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<(IEnumerable<Reservation> Items, int TotalCount)> GetFilteredPagedAsync(
        int? hotelId, DateTime? from, DateTime? to, ReservationStatus? status,
        int page, int pageSize)
    {
        var query = _context.Reservations
            .Include(r => r.RoomType)
            .ThenInclude(rt => rt.Hotel)
            .AsQueryable();

        if (hotelId.HasValue)
            query = query.Where(r => r.RoomType.HotelId == hotelId.Value);

        if (from.HasValue)
            query = query.Where(r => r.CheckIn >= from.Value);

        if (to.HasValue)
            query = query.Where(r => r.CheckOut <= to.Value);

        if (status.HasValue)
            query = query.Where(r => r.Status == status.Value);

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(r => r.CheckIn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<Reservation> CreateAsync(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<Reservation> UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
        return reservation;
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Reservations.CountAsync();
    }

    public async Task<IEnumerable<Reservation>> GetRecentAsync(int count)
    {
        return await _context.Reservations
            .OrderByDescending(r => r.Id)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetActiveByHotelAndDateRangeAsync(int hotelId, DateTime from, DateTime to)
    {
        return await _context.Reservations
            .Include(r => r.RoomType)
            .Where(r => r.RoomType.HotelId == hotelId
                && r.Status != ReservationStatus.Cancelled
                && r.CheckIn <= to
                && r.CheckOut > from)
            .ToListAsync();
    }

    public async Task<int> GetTotalRoomsBookedTodayAsync()
    {
        var today = DateTime.UtcNow.Date;
        return await _context.Reservations
            .Where(r => r.Status != ReservationStatus.Cancelled
                && r.CheckIn <= today
                && r.CheckOut > today)
            .SumAsync(r => r.RoomsBooked);
    }

    public async Task<Dictionary<ReservationStatus, int>> GetCountByStatusAsync()
    {
        return await _context.Reservations
            .GroupBy(r => r.Status)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }
}
