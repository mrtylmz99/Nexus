# 🚀 Project Nexus

Project Nexus is a modern, full-stack Task and Project Management application built with **.NET 9** (Backend) and **Angular** (Frontend). It follows **Clean Architecture** principles and implements best practices like **SOLID**, **CQRS** (ready), and **Dependency Injection**.

## 🛠️ Technology Stack

### Backend (.NET 9)

- **ASP.NET Core Web API**: High-performance RESTful API.
- **Entity Framework Core 9**: ORM for database access (SQL Server).
- **Serilog**: Robust logging to SQL Server (`AppLogs` table).
- **Clean Architecture**: Separation of concerns (Domain, Application, Infrastructure, API).
- **xUnit & Moq**: Unit testing for business logic.
- **Scalar**: Modern API documentation and testing UI.

### Frontend (Angular)

- **Angular 19**: Component-based UI framework.
- **TailwindCSS**: Utility-first CSS framework for styling.

## 🏗️ Architecture

The solution maps strictly to Clean Architecture layers:

1.  **Nexus.Domain**: Core entities (`Project`, `TaskItem`) and Enums. No dependencies.
2.  **Nexus.Application**: Business logic interfaces (`IProjectService`) and DTOs. Depends on Domain.
3.  **Nexus.Infrastructure**: Implementation of interfaces, EF Core `DbContext`, and Migrations. Depends on Application.
4.  **Nexus.API**: The entry point (Controllers). Depends on Application and Infrastructure.

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js & npm
- SQL Server (LocalDB or Express)

### Database Setup

The application uses **SQL Server**. The connection string is configured in `appsettings.json`.
It defaults to `.\SQLEXPRESS`. If you use LocalDB, change it to `(localdb)\mssqllocaldb`.

```bash
# Apply Migrations and Update Database
dotnet ef database update --project Nexus.Infrastructure --startup-project Nexus.API
```

### Running the Backend

```bash
cd Nexus.API
dotnet run
```

The API will be available at `https://localhost:xxxx`.
**Documentation**: Visit `/scalar/v1` to see the beautiful API documentation.

## 🧪 Testing

To run the unit tests:

```bash
dotnet test
```

## 📝 Logging

All application logs and errors are automatically saved to the `AppLogs` table in the database.

---

