# Forklift Control System (Backend)

A backend system for managing and controlling forklifts, built with Clean Architecture principles using .NET, Entity Framework Core, and SQL Server.

## Project Overview
This project provides a backend API for forklift management and control. It is structured using Clean Architecture, separating concerns across Application, Domain, Infrastructure, and Presentation layers. Clean Architecture ensures maintainability, testability, and scalability by organizing code into clear, independent layers.

## Features
- Forklift list
- Command processing for forklifts
- Exception handling and request/response logging middleware
- Extensible architecture for future features

## Technologies Used
- .NET 8
- Clean Architecture
- Entity Framework Core
- SQL Server (database migrations present, but not currently used)

## Getting Started

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download)
- (Optional) SQL Server (for database migrations, not currently used)

### Setup
1. Clone the repository:
   ```bash
   git clone <repo-url>
   cd ForkliftControlSystem
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Build the project:
   ```bash
   dotnet build
   ```
4. Run the project:
   ```bash
   dotnet run --project ForkliftControlSystem/ForkliftControlSystem.csproj
   ```

### Database
- The project includes Entity Framework Core migrations for a forklifts table, but the database is not currently used in the application workflow. You do not need to set up SQL Server to run or test the current backend.

## Project Structure
```
ForkliftControlSystem/
  ForkliftControlSystem/
    Application/      # Application logic, DTOs, interfaces
    Domain/           # Domain entities, enums, exceptions
    Infrastructure/   # Data access, services, repositories
    Presentation/     # Controllers, middleware
    Program.cs        # Entry point
    ...
  README.md
  ForkliftControlSystem.sln
```

## API Endpoints
- The backend exposes endpoints for forklift and command management.
- See `Presentation/Controllers/` for details.

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](LICENSE) (or specify your license)
