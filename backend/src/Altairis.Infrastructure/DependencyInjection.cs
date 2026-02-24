using Altairis.Application.Interfaces;
using Altairis.Application.Services;
using Altairis.Infrastructure.Data;
using Altairis.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AltairisDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IRoomCategoryRepository, RoomCategoryRepository>();
        services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();

        services.AddScoped<HotelService>();
        services.AddScoped<RoomCategoryService>();
        services.AddScoped<RoomTypeService>();
        services.AddScoped<ReservationService>();
        services.AddScoped<DashboardService>();
        services.AddScoped<AvailabilityService>();

        return services;
    }
}
