FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for restore
COPY src/Altairis.Domain/Altairis.Domain.csproj src/Altairis.Domain/
COPY src/Altairis.Application/Altairis.Application.csproj src/Altairis.Application/
COPY src/Altairis.Infrastructure/Altairis.Infrastructure.csproj src/Altairis.Infrastructure/
COPY src/Altairis.Api/Altairis.Api.csproj src/Altairis.Api/

# Restore
RUN dotnet restore src/Altairis.Api/Altairis.Api.csproj

# Copy everything else and build
COPY . .
RUN dotnet publish src/Altairis.Api/Altairis.Api.csproj -c Release -o /app/publish --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Altairis.Api.dll"]
