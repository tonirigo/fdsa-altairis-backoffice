# Altairis Backoffice

Panel de administración para la gestión hotelera de Altairis. Permite gestionar hoteles, tipos de habitación, categorías, reservas e inventario de disponibilidad.

## Stack

- **Backend:** .NET 8 (Clean Architecture) con Entity Framework Core y SQL Server
- **Frontend:** Angular 21 con Angular Material
- **Base de datos:** SQL Server 2022
- **Contenedores:** Docker Compose

## Requisitos

- Docker y Docker Compose

## Cómo levantar el proyecto

```bash
docker compose up -d --build
```

Esto levanta tres servicios:

- **SQL Server** en `localhost:1433`
- **Backend API** en `http://localhost:5000`
- **Frontend** en `http://localhost:4200`

La base de datos se crea automáticamente con datos de prueba al arrancar el backend (EF Core migrations + seed).

## Estructura del proyecto

```
backend/
  src/
    Altairis.Api/           # Controllers y configuración de la API
    Altairis.Application/   # DTOs, interfaces y servicios de aplicación
    Altairis.Domain/        # Entidades y enums del dominio
    Altairis.Infrastructure/ # DbContext y repositorios
frontend/
  altairis-frontend/        # Aplicación Angular
docker-compose.yml
```

## Funcionalidades

- **Dashboard** con estadísticas generales y gráficos de ocupación
- **Hoteles** — CRUD completo con búsqueda y paginación
- **Tipos de habitación** — Asignación por hotel con categorías
- **Categorías de habitación** — Gestión de categorías (Standard, Suite, etc.)
- **Reservas** — Creación, listado con filtros, confirmación y cancelación
- **Inventario** — Vista de disponibilidad por hotel, tipo de habitación y rango de fechas

## API

El backend expone una API REST documentada con Swagger, accesible en `http://localhost:5000/swagger` cuando el entorno es Development.
