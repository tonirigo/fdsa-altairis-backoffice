using Altairis.Domain.Enums;

namespace Altairis.Application.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public int RoomsBooked { get; set; }
    public ReservationStatus Status { get; set; }
    public string? RoomTypeName { get; set; }
    public string? HotelName { get; set; }
}

public class CreateReservationDto
{
    public int RoomTypeId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public int RoomsBooked { get; set; }
}

public class UpdateReservationStatusDto
{
    public ReservationStatus Status { get; set; }
}
