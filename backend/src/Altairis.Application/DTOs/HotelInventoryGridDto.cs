namespace Altairis.Application.DTOs;

public class HotelInventoryGridDto
{
    public int HotelId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public List<RoomTypeInventoryRow> RoomTypes { get; set; } = new();
}

public class RoomTypeInventoryRow
{
    public int RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public string? CategoryName { get; set; }
    public int Capacity { get; set; }
    public int TotalRooms { get; set; }
    public List<InventoryCell> Dates { get; set; } = new();
}

public class InventoryCell
{
    public DateTime Date { get; set; }
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
}
