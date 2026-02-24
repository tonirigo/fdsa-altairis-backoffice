using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;

namespace Altairis.Application.Services;

public class AvailabilityService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;

    public AvailabilityService(IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository)
    {
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<HotelInventoryGridDto> GetHotelGridAsync(int hotelId, DateTime from, DateTime to)
    {
        var roomTypes = await _roomTypeRepository.GetByHotelIdAsync(hotelId);
        var reservations = await _reservationRepository.GetActiveByHotelAndDateRangeAsync(hotelId, from, to);

        var allDates = Enumerable.Range(0, (to - from).Days + 1)
            .Select(d => from.AddDays(d))
            .ToList();

        var reservationLookup = reservations.GroupBy(r => r.RoomTypeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var rows = roomTypes.Select(rt =>
        {
            reservationLookup.TryGetValue(rt.Id, out var rtReservations);
            rtReservations ??= new List<Domain.Entities.Reservation>();

            return new RoomTypeInventoryRow
            {
                RoomTypeId = rt.Id,
                RoomTypeName = rt.Name,
                CategoryName = rt.Category?.Name,
                Capacity = rt.Capacity,
                TotalRooms = rt.TotalRooms,
                Dates = allDates.Select(date =>
                {
                    var booked = rtReservations
                        .Where(r => r.CheckIn.Date <= date.Date && r.CheckOut.Date > date.Date)
                        .Sum(r => r.RoomsBooked);

                    return new InventoryCell
                    {
                        Date = date,
                        TotalRooms = rt.TotalRooms,
                        AvailableRooms = Math.Max(0, rt.TotalRooms - booked)
                    };
                }).ToList()
            };
        }).OrderBy(r => r.RoomTypeName).ToList();

        return new HotelInventoryGridDto
        {
            HotelId = hotelId,
            From = from,
            To = to,
            RoomTypes = rows
        };
    }
}
