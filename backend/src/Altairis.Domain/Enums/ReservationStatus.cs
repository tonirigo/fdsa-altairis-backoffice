using System.Text.Json.Serialization;

namespace Altairis.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled
}
