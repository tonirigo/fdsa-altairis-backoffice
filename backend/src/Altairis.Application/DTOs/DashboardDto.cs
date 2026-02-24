namespace Altairis.Application.DTOs;

public class DashboardDto
{
    public int TotalHotels { get; set; }
    public int TotalReservations { get; set; }
    public int AvailableRoomsToday { get; set; }
    public double OccupancyPercentage { get; set; }
    public List<ReservationDto> RecentReservations { get; set; } = new();
    public List<HotelOccupancyDto> OccupancyByHotel { get; set; } = new();
    public ReservationsByStatusDto ReservationsByStatus { get; set; } = new();
}

public class HotelOccupancyDto
{
    public string HotelName { get; set; } = string.Empty;
    public int TotalRooms { get; set; }
    public int BookedRooms { get; set; }
    public double OccupancyPercentage { get; set; }
}

public class ReservationsByStatusDto
{
    public int Pending { get; set; }
    public int Confirmed { get; set; }
    public int Cancelled { get; set; }
}
