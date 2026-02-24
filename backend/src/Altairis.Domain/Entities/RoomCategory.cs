namespace Altairis.Domain.Entities;

public class RoomCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<RoomType> RoomTypes { get; set; } = new List<RoomType>();
}
