using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;
using Altairis.Domain.Enums;

namespace Altairis.Application.Services;

public class DashboardService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;

    public DashboardService(
        IHotelRepository hotelRepository,
        IReservationRepository reservationRepository,
        IRoomTypeRepository roomTypeRepository)
    {
        _hotelRepository = hotelRepository;
        _reservationRepository = reservationRepository;
        _roomTypeRepository = roomTypeRepository;
    }

    public async Task<DashboardDto> GetDashboardAsync()
    {
        var hotels = await _hotelRepository.GetAllAsync();
        var totalReservations = await _reservationRepository.GetTotalCountAsync();
        var totalRooms = await _roomTypeRepository.GetTotalRoomsAsync();
        var bookedToday = await _reservationRepository.GetTotalRoomsBookedTodayAsync();
        var recentReservations = await _reservationRepository.GetRecentAsync(5);

        var availableToday = Math.Max(0, totalRooms - bookedToday);
        var occupancyPercentage = totalRooms > 0
            ? Math.Round((double)bookedToday / totalRooms * 100, 1)
            : 0;

        // Occupancy by hotel
        var allRoomTypes = await _roomTypeRepository.GetAllWithHotelAsync();
        var today = DateTime.UtcNow.Date;

        var hotelGroups = allRoomTypes.GroupBy(rt => new { rt.HotelId, rt.Hotel.Name });
        var occupancyByHotel = new List<HotelOccupancyDto>();

        foreach (var group in hotelGroups)
        {
            var hotelTotalRooms = group.Sum(rt => rt.TotalRooms);
            var activeReservations = await _reservationRepository.GetActiveByHotelAndDateRangeAsync(
                group.Key.HotelId, today, today.AddDays(1));
            var hotelBookedRooms = activeReservations.Sum(r => r.RoomsBooked);

            occupancyByHotel.Add(new HotelOccupancyDto
            {
                HotelName = group.Key.Name,
                TotalRooms = hotelTotalRooms,
                BookedRooms = Math.Min(hotelBookedRooms, hotelTotalRooms),
                OccupancyPercentage = hotelTotalRooms > 0
                    ? Math.Round((double)hotelBookedRooms / hotelTotalRooms * 100, 1)
                    : 0
            });
        }

        // Reservations by status
        var statusCounts = await _reservationRepository.GetCountByStatusAsync();
        var reservationsByStatus = new ReservationsByStatusDto
        {
            Pending = statusCounts.GetValueOrDefault(ReservationStatus.Pending, 0),
            Confirmed = statusCounts.GetValueOrDefault(ReservationStatus.Confirmed, 0),
            Cancelled = statusCounts.GetValueOrDefault(ReservationStatus.Cancelled, 0)
        };

        return new DashboardDto
        {
            TotalHotels = hotels.Count(),
            TotalReservations = totalReservations,
            AvailableRoomsToday = availableToday,
            OccupancyPercentage = occupancyPercentage,
            RecentReservations = recentReservations.Select(r => new ReservationDto
            {
                Id = r.Id,
                RoomTypeId = r.RoomTypeId,
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                GuestName = r.GuestName,
                RoomsBooked = r.RoomsBooked,
                Status = r.Status
            }).ToList(),
            OccupancyByHotel = occupancyByHotel
                .OrderByDescending(h => h.OccupancyPercentage)
                .Take(10)
                .ToList(),
            ReservationsByStatus = reservationsByStatus
        };
    }
}
