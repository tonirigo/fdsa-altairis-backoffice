using System.Reflection;
using System.Text.Json.Serialization;
using Altairis.Infrastructure;
using Altairis.Infrastructure.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Altairis Backoffice API",
        Version = "v1",
        Description = "B2B hotel distribution backoffice API"
    });
    options.UseInlineDefinitionsForEnums();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Add Infrastructure (DbContext, repositories, services)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddInfrastructure(connectionString);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Auto-create database on startup with retry for Docker startup order
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AltairisDbContext>();
    var retries = 10;
    for (var i = 0; i < retries; i++)
    {
        try
        {
            context.Database.EnsureCreated();
            break;
        }
        catch (Microsoft.Data.SqlClient.SqlException) when (i < retries - 1)
        {
            Thread.Sleep(3000);
        }
    }
}

// Swagger always available (UI only in development)
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.MapControllers();

app.Run();
