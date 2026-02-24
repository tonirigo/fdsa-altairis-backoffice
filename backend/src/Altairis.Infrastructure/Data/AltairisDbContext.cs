using Altairis.Domain.Entities;
using Altairis.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Altairis.Infrastructure.Data;

public class AltairisDbContext : DbContext
{
    public AltairisDbContext(DbContextOptions<AltairisDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<RoomCategory> RoomCategories => Set<RoomCategory>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(300);
            entity.HasMany(e => e.RoomTypes).WithOne(r => r.Hotel).HasForeignKey(r => r.HotelId);
        });

        modelBuilder.Entity<RoomCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.RoomTypes).WithOne(r => r.Category).HasForeignKey(r => r.CategoryId);
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(e => e.Reservations).WithOne(r => r.RoomType).HasForeignKey(r => r.RoomTypeId);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.GuestName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        });

        // Seed data
        var today = new DateTime(2026, 2, 24);

        modelBuilder.Entity<Hotel>().HasData(
            new Hotel { Id = 1, Name = "Grand Palace Hotel", Country = "Spain", City = "Barcelona", Address = "La Rambla 45", CreatedAt = today.AddMonths(-6) },
            new Hotel { Id = 2, Name = "Mountain View Resort", Country = "Switzerland", City = "Zurich", Address = "Bahnhofstrasse 12", CreatedAt = today.AddMonths(-5) },
            new Hotel { Id = 3, Name = "Seaside Inn", Country = "Italy", City = "Amalfi", Address = "Via Marina 8", CreatedAt = today.AddMonths(-4) },
            new Hotel { Id = 4, Name = "The Ritz Madrid", Country = "Spain", City = "Madrid", Address = "Paseo del Prado 28", CreatedAt = today.AddMonths(-3) },
            new Hotel { Id = 5, Name = "Le Marais Boutique", Country = "France", City = "Paris", Address = "Rue de Rivoli 112", CreatedAt = today.AddMonths(-3) },
            new Hotel { Id = 6, Name = "Porto Riverside Hotel", Country = "Portugal", City = "Porto", Address = "Cais da Ribeira 20", CreatedAt = today.AddMonths(-2) },
            new Hotel { Id = 7, Name = "Berlin Central Suites", Country = "Germany", City = "Berlin", Address = "Friedrichstrasse 88", CreatedAt = today.AddMonths(-2) },
            new Hotel { Id = 8, Name = "Amsterdam Canal House", Country = "Netherlands", City = "Amsterdam", Address = "Herengracht 315", CreatedAt = today.AddMonths(-1) },
            new Hotel { Id = 9, Name = "Vienna Imperial Lodge", Country = "Austria", City = "Vienna", Address = "Ringstrasse 5", CreatedAt = today.AddMonths(-1) },
            new Hotel { Id = 10, Name = "Lisbon Alfama Retreat", Country = "Portugal", City = "Lisbon", Address = "Rua Augusta 47", CreatedAt = today.AddDays(-15) }
        );

        modelBuilder.Entity<RoomCategory>().HasData(
            new RoomCategory { Id = 1, Name = "Standard" },
            new RoomCategory { Id = 2, Name = "Suite" },
            new RoomCategory { Id = 3, Name = "Deluxe" },
            new RoomCategory { Id = 4, Name = "Family" },
            new RoomCategory { Id = 5, Name = "Penthouse" }
        );

        modelBuilder.Entity<RoomType>().HasData(
            // Grand Palace Hotel (Barcelona)
            new RoomType { Id = 1, HotelId = 1, CategoryId = 1, Name = "Standard Double", Capacity = 2, TotalRooms = 30 },
            new RoomType { Id = 2, HotelId = 1, CategoryId = 2, Name = "Junior Suite", Capacity = 3, TotalRooms = 12 },
            new RoomType { Id = 3, HotelId = 1, CategoryId = 5, Name = "Royal Penthouse", Capacity = 4, TotalRooms = 2 },
            // Mountain View Resort (Zurich)
            new RoomType { Id = 4, HotelId = 2, CategoryId = 3, Name = "Deluxe Alpine", Capacity = 2, TotalRooms = 20 },
            new RoomType { Id = 5, HotelId = 2, CategoryId = 4, Name = "Family Chalet", Capacity = 6, TotalRooms = 8 },
            // Seaside Inn (Amalfi)
            new RoomType { Id = 6, HotelId = 3, CategoryId = 1, Name = "Ocean View Standard", Capacity = 2, TotalRooms = 15 },
            new RoomType { Id = 7, HotelId = 3, CategoryId = 3, Name = "Cliff Deluxe", Capacity = 2, TotalRooms = 6 },
            // The Ritz Madrid
            new RoomType { Id = 8, HotelId = 4, CategoryId = 1, Name = "Classic Room", Capacity = 2, TotalRooms = 40 },
            new RoomType { Id = 9, HotelId = 4, CategoryId = 2, Name = "Grand Suite", Capacity = 4, TotalRooms = 10 },
            new RoomType { Id = 10, HotelId = 4, CategoryId = 5, Name = "Presidential Suite", Capacity = 6, TotalRooms = 2 },
            // Le Marais Boutique (Paris)
            new RoomType { Id = 11, HotelId = 5, CategoryId = 3, Name = "Parisian Deluxe", Capacity = 2, TotalRooms = 18 },
            new RoomType { Id = 12, HotelId = 5, CategoryId = 2, Name = "Eiffel Suite", Capacity = 3, TotalRooms = 5 },
            // Porto Riverside Hotel
            new RoomType { Id = 13, HotelId = 6, CategoryId = 1, Name = "River View Standard", Capacity = 2, TotalRooms = 25 },
            new RoomType { Id = 14, HotelId = 6, CategoryId = 4, Name = "Family River Suite", Capacity = 5, TotalRooms = 8 },
            // Berlin Central Suites
            new RoomType { Id = 15, HotelId = 7, CategoryId = 1, Name = "City Standard", Capacity = 2, TotalRooms = 35 },
            new RoomType { Id = 16, HotelId = 7, CategoryId = 3, Name = "Executive Deluxe", Capacity = 2, TotalRooms = 15 },
            // Amsterdam Canal House
            new RoomType { Id = 17, HotelId = 8, CategoryId = 3, Name = "Canal View Deluxe", Capacity = 2, TotalRooms = 12 },
            new RoomType { Id = 18, HotelId = 8, CategoryId = 2, Name = "Heritage Suite", Capacity = 3, TotalRooms = 4 },
            // Vienna Imperial Lodge
            new RoomType { Id = 19, HotelId = 9, CategoryId = 1, Name = "Imperial Standard", Capacity = 2, TotalRooms = 28 },
            new RoomType { Id = 20, HotelId = 9, CategoryId = 5, Name = "Mozart Penthouse", Capacity = 4, TotalRooms = 3 },
            // Lisbon Alfama Retreat
            new RoomType { Id = 21, HotelId = 10, CategoryId = 1, Name = "Alfama Standard", Capacity = 2, TotalRooms = 20 },
            new RoomType { Id = 22, HotelId = 10, CategoryId = 3, Name = "Tejo View Deluxe", Capacity = 2, TotalRooms = 10 }
        );

        modelBuilder.Entity<Reservation>().HasData(
            // Grand Palace Hotel — Barcelona
            new Reservation { Id = 1, RoomTypeId = 1, CheckIn = today, CheckOut = today.AddDays(3), GuestName = "John Smith", RoomsBooked = 2, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 2, RoomTypeId = 2, CheckIn = today.AddDays(1), CheckOut = today.AddDays(4), GuestName = "Sophie Laurent", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 3, RoomTypeId = 1, CheckIn = today.AddDays(2), CheckOut = today.AddDays(5), GuestName = "Carlos Mendez", RoomsBooked = 3, Status = ReservationStatus.Pending },
            new Reservation { Id = 4, RoomTypeId = 3, CheckIn = today.AddDays(5), CheckOut = today.AddDays(8), GuestName = "Emma Wilson", RoomsBooked = 1, Status = ReservationStatus.Pending },
            // Mountain View Resort — Zurich
            new Reservation { Id = 5, RoomTypeId = 4, CheckIn = today.AddDays(-2), CheckOut = today.AddDays(1), GuestName = "Hans Mueller", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 6, RoomTypeId = 5, CheckIn = today, CheckOut = today.AddDays(7), GuestName = "Yuki Tanaka", RoomsBooked = 2, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 7, RoomTypeId = 4, CheckIn = today.AddDays(3), CheckOut = today.AddDays(6), GuestName = "Pierre Dubois", RoomsBooked = 1, Status = ReservationStatus.Pending },
            // Seaside Inn — Amalfi
            new Reservation { Id = 8, RoomTypeId = 6, CheckIn = today.AddDays(1), CheckOut = today.AddDays(5), GuestName = "Maria Garcia", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 9, RoomTypeId = 7, CheckIn = today.AddDays(-1), CheckOut = today.AddDays(3), GuestName = "Luca Rossi", RoomsBooked = 1, Status = ReservationStatus.Cancelled },
            new Reservation { Id = 10, RoomTypeId = 6, CheckIn = today.AddDays(4), CheckOut = today.AddDays(7), GuestName = "Anna Kowalski", RoomsBooked = 2, Status = ReservationStatus.Pending },
            // The Ritz Madrid
            new Reservation { Id = 11, RoomTypeId = 8, CheckIn = today, CheckOut = today.AddDays(2), GuestName = "David Chen", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 12, RoomTypeId = 9, CheckIn = today.AddDays(1), CheckOut = today.AddDays(5), GuestName = "Isabel Fernandez", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 13, RoomTypeId = 8, CheckIn = today.AddDays(3), CheckOut = today.AddDays(6), GuestName = "Michael Brown", RoomsBooked = 4, Status = ReservationStatus.Pending },
            new Reservation { Id = 14, RoomTypeId = 10, CheckIn = today.AddDays(7), CheckOut = today.AddDays(10), GuestName = "Sarah Johnson", RoomsBooked = 1, Status = ReservationStatus.Pending },
            // Le Marais Boutique — Paris
            new Reservation { Id = 15, RoomTypeId = 11, CheckIn = today.AddDays(-3), CheckOut = today, GuestName = "Oliver Martin", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 16, RoomTypeId = 12, CheckIn = today.AddDays(2), CheckOut = today.AddDays(5), GuestName = "Camille Dupont", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 17, RoomTypeId = 11, CheckIn = today.AddDays(1), CheckOut = today.AddDays(4), GuestName = "James Taylor", RoomsBooked = 2, Status = ReservationStatus.Cancelled },
            // Porto Riverside Hotel
            new Reservation { Id = 18, RoomTypeId = 13, CheckIn = today, CheckOut = today.AddDays(3), GuestName = "Ana Silva", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 19, RoomTypeId = 14, CheckIn = today.AddDays(2), CheckOut = today.AddDays(6), GuestName = "Joao Pereira", RoomsBooked = 1, Status = ReservationStatus.Pending },
            // Berlin Central Suites
            new Reservation { Id = 20, RoomTypeId = 15, CheckIn = today.AddDays(1), CheckOut = today.AddDays(4), GuestName = "Thomas Weber", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 21, RoomTypeId = 16, CheckIn = today.AddDays(-1), CheckOut = today.AddDays(2), GuestName = "Lisa Schmidt", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 22, RoomTypeId = 15, CheckIn = today.AddDays(5), CheckOut = today.AddDays(8), GuestName = "Robert Kim", RoomsBooked = 2, Status = ReservationStatus.Pending },
            // Amsterdam Canal House
            new Reservation { Id = 23, RoomTypeId = 17, CheckIn = today, CheckOut = today.AddDays(3), GuestName = "Erik van den Berg", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 24, RoomTypeId = 18, CheckIn = today.AddDays(3), CheckOut = today.AddDays(7), GuestName = "Marie Jansen", RoomsBooked = 1, Status = ReservationStatus.Pending },
            // Vienna Imperial Lodge
            new Reservation { Id = 25, RoomTypeId = 19, CheckIn = today.AddDays(-2), CheckOut = today.AddDays(1), GuestName = "Franz Gruber", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 26, RoomTypeId = 20, CheckIn = today.AddDays(1), CheckOut = today.AddDays(4), GuestName = "Elena Popov", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 27, RoomTypeId = 19, CheckIn = today.AddDays(4), CheckOut = today.AddDays(7), GuestName = "Ahmed Hassan", RoomsBooked = 3, Status = ReservationStatus.Pending },
            // Lisbon Alfama Retreat
            new Reservation { Id = 28, RoomTypeId = 21, CheckIn = today, CheckOut = today.AddDays(4), GuestName = "Pedro Costa", RoomsBooked = 1, Status = ReservationStatus.Confirmed },
            new Reservation { Id = 29, RoomTypeId = 22, CheckIn = today.AddDays(2), CheckOut = today.AddDays(5), GuestName = "Rita Santos", RoomsBooked = 1, Status = ReservationStatus.Pending },
            new Reservation { Id = 30, RoomTypeId = 21, CheckIn = today.AddDays(-4), CheckOut = today.AddDays(-1), GuestName = "Mark Anderson", RoomsBooked = 2, Status = ReservationStatus.Cancelled }
        );
    }
}
