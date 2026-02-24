using Altairis.Domain.Entities;
using Altairis.Domain.Enums;

namespace Altairis.Application.Interfaces;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync(int id);
    Task<(IEnumerable<Reservation> Items, int TotalCount)> GetFilteredPagedAsync(
        int? hotelId, DateTime? from, DateTime? to, ReservationStatus? status,
        int page, int pageSize);
    Task<Reservation> CreateAsync(Reservation reservation);
    Task<Reservation> UpdateAsync(Reservation reservation);
    Task<int> GetTotalCountAsync();
    Task<IEnumerable<Reservation>> GetRecentAsync(int count);
    Task<IEnumerable<Reservation>> GetActiveByHotelAndDateRangeAsync(int hotelId, DateTime from, DateTime to);
    Task<int> GetTotalRoomsBookedTodayAsync();
    Task<Dictionary<ReservationStatus, int>> GetCountByStatusAsync();
}
