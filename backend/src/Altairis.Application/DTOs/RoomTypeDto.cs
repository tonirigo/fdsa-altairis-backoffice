namespace Altairis.Application.DTOs;

public class RoomTypeDto
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int TotalRooms { get; set; }
}

public class CreateRoomTypeDto
{
    public int? CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int TotalRooms { get; set; }
}
