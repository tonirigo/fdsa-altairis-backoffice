namespace Altairis.Domain.Entities;

public class RoomType
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int TotalRooms { get; set; }
    public Hotel Hotel { get; set; } = null!;
    public RoomCategory? Category { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
