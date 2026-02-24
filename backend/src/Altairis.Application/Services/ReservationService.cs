using Altairis.Application.DTOs;
using Altairis.Application.Interfaces;
using Altairis.Domain.Entities;
using Altairis.Domain.Enums;

namespace Altairis.Application.Services;

public class ReservationService
{
    private readonly IReservationRepository _repository;

    public ReservationService(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReservationDto>> GetAllAsync()
    {
        var reservations = await _repository.GetAllAsync();
        return reservations.Select(MapToDto);
    }

    public async Task<ReservationDto?> GetByIdAsync(int id)
    {
        var reservation = await _repository.GetByIdAsync(id);
        if (reservation == null)
            return null;

        return MapToDto(reservation);
    }

    public async Task<PagedResult<ReservationDto>> GetFilteredAsync(
        int? hotelId, DateTime? from, DateTime? to, ReservationStatus? status,
        int page, int pageSize)
    {
        var (items, totalCount) = await _repository.GetFilteredPagedAsync(
            hotelId, from, to, status, page, pageSize);

        return new PagedResult<ReservationDto>
        {
            Items = items.Select(MapToDto).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ReservationDto> CreateAsync(CreateReservationDto dto)
    {
        var reservation = new Reservation
        {
            RoomTypeId = dto.RoomTypeId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            GuestName = dto.GuestName,
            RoomsBooked = dto.RoomsBooked,
            Status = ReservationStatus.Pending
        };

        var created = await _repository.CreateAsync(reservation);
        return MapToDto(created);
    }

    public async Task<ReservationDto?> UpdateStatusAsync(int id, UpdateReservationStatusDto dto)
    {
        var reservation = await _repository.GetByIdAsync(id);
        if (reservation == null)
            return null;

        reservation.Status = dto.Status;
        var updated = await _repository.UpdateAsync(reservation);
        return MapToDto(updated);
    }

    private static ReservationDto MapToDto(Reservation reservation) => new()
    {
        Id = reservation.Id,
        RoomTypeId = reservation.RoomTypeId,
        CheckIn = reservation.CheckIn,
        CheckOut = reservation.CheckOut,
        GuestName = reservation.GuestName,
        RoomsBooked = reservation.RoomsBooked,
        Status = reservation.Status,
        RoomTypeName = reservation.RoomType?.Name,
        HotelName = reservation.RoomType?.Hotel?.Name
    };
}
