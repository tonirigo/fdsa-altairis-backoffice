using Altairis.Domain.Enums;

namespace Altairis.Domain.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int RoomTypeId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public int RoomsBooked { get; set; }
    public ReservationStatus Status { get; set; }
    public RoomType RoomType { get; set; } = null!;
}
