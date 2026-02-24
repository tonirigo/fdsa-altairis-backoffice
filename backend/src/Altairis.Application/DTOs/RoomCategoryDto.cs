namespace Altairis.Application.DTOs;

public class RoomCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CreateRoomCategoryDto
{
    public string Name { get; set; } = string.Empty;
}
