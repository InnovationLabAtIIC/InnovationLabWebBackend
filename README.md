# InnovationLab Backend

A comprehensive microservices-based backend system for an innovation lab platform, built with .NET 8 and PostgreSQL.

## Project Overview

InnovationLab Backend is a modular web API system designed to support an innovation lab platform. The project follows a microservices architecture with three main services:

- **Auth API** - Handles user authentication, authorization, and identity management
- **Landing API** - Manages landing page content and general information
- **Learn API** - Provides learning resources and educational content management

## Architecture

The project follows a clean architecture pattern with the following structure:

```
InnovationLabWebBackend/
├── InnovationLab.Shared/          # Common utilities and extensions
├── InnovationLab.Auth/            # Authentication service
├── InnovationLab.Landing/         # Landing page service
├── InnovationLab.Learn/           # Learning content service
├── compose.yml                    # Docker Compose configuration
└── InnovationLab.sln             # Solution file
```

### Key Features

- **Microservices Architecture**: Separate services for different domains
- **JWT Authentication**: Secure token-based authentication
- **Entity Framework Core**: Database ORM with Code First migrations
- **PostgreSQL Database**: Robust relational database
- **Docker Support**: Containerized deployment
- **Swagger Integration**: API documentation and testing
- **Shared Library**: Common functionality across services

## Technology Stack

- **.NET 8**: Latest long-term support version
- **ASP.NET Core Web API**: REST API framework
- **Entity Framework Core 8.0**: Object-relational mapping
- **PostgreSQL 17.6**: Primary database
- **ASP.NET Core Identity**: User management and authentication
- **JWT Bearer Tokens**: Secure authentication
- **Docker & Docker Compose**: Containerization
- **Swagger/OpenAPI**: API documentation
- **Npgsql**: PostgreSQL .NET driver

## Prerequisites

Before running this project, ensure you have the following installed:

### For Local Development (dotnet run)

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (8.0 or later)
- [PostgreSQL 17.6](https://www.postgresql.org/download/) (or compatible version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (recommended)
- [Git](https://git-scm.com/downloads)

### For Docker Development

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (latest version)
- [Git](https://git-scm.com/downloads)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/InnovationLabAtIIC/InnovationLabWebBackend.git
cd InnovationLabWebBackend
```

### 2. Environment Configuration

Create environment variables file `.env` in the root of the project

```env
PG_USER=<POSTGRES_USERNAME>
PG_PASSWORD=<POSTGRES_PASSWORD>
PG_DATABASE=<POSTGRES_DATABASE>
```

Then, create `appsettings.json` file in the root of the project

```json
{
  "ConnectionStrings": {
    "PostgresConnection": "Host=<HOSTNAME>:<PORT>;Database=<POSTGRES_DATABASE>;Username=<POSTGRES_USERNAME>;Password=<POSTGRES_PASSWORD>;"
  },
  "Jwt": {
    "Secret": "<JWT_256_BIT_SECRET_KEY>",
    "Issuer": "<JWT_ISSUER>",
    "Audience": "<JWT_AUDIENCE>",
    "ExpiryMinutes": <JWT_EXPIRY_IN_MINUTES>
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Note:** Replace the variable names enclosed with angular brackets `<...>` with your actual values.

## Running the Application

You can run the application in two ways: locally using .NET CLI or using Docker Compose.

### Option 1: Running Locally with .NET CLI

#### Step 1: Setup PostgreSQL Database

1. **Install PostgreSQL** if not already installed
2. **Create the database**:

   ```sql
   CREATE DATABASE <POSTGRES_DATABASE>;
   CREATE USER <POSTGRES_USERNAME> WITH PASSWORD '<POSTGRES_PASSWORD>';
   GRANT ALL PRIVILEGES ON DATABASE <POSTGRES_DATABASE> TO <POSTGRES_USERNAME>;
   ```

**Note:** Replace the variable names enclosed with angular brackets `<...>` with your actual values.

#### Step 2: Restore Dependencies

```bash
dotnet restore
```

#### Step 3: Run Database Migrations

Navigate to each service directory and run migrations:

```bash
cd InnovationLab.Auth
dotnet ef database update
cd ..

cd InnovationLab.Landing
dotnet ef database update
cd ..

cd InnovationLab.Learn
dotnet ef database update
cd ..
```

#### Step 4: Run the Services

Open separate terminal windows for each service:

**Terminal 1 - Auth Service:**

```bash
cd InnovationLab.Auth
dotnet run
```

The Auth API will be available at: `https://localhost:5001`

**Terminal 2 - Landing Service:**

```bash
cd InnovationLab.Landing
dotnet run
```

The Landing API will be available at: `https://localhost:5002`

**Terminal 3 - Learn Service:**

```bash
cd InnovationLab.Learn
dotnet run
```

The Learn API will be available at: `https://localhost:5003`

### Option 2: Running with Docker Compose

#### Step 1: Build and Run with Docker Compose

```bash
docker compose up --build -d
```

#### Step 3: Verify Services

After Docker Compose starts successfully, the services will be available at:

- **Auth API**: `http://localhost:5001`
- **Landing API**: `http://localhost:5002`
- **Learn API**: `http://localhost:5003`
- **PostgreSQL Database**: `localhost:5432`

#### Step 4: Stop Services

To stop all services:

```bash
docker compose down
```

Or

```bash
# To also remove volumes (database data)
docker compose down -v
```

## API Documentation

Once the services are running, you can access the Swagger documentation:

- **Auth API Docs**: `http://localhost:5001/swagger`
- **Landing API Docs**: `http://localhost:5002/swagger`
- **Learn API Docs**: `http://localhost:5003/swagger`

## Database Migrations

### Creating New Migrations

When you make changes to entity models, create new migrations:

```bash
# Example for Auth service
cd InnovationLab.Auth
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Viewing Migration History

```bash
cd [ServiceDirectory]
dotnet ef migrations list
```

## Project Structure Details

### InnovationLab.Shared

Contains common utilities, extensions, and shared functionality used across all services.

### InnovationLab.Auth

- User registration and authentication
- JWT token generation and validation
- Identity management with ASP.NET Core Identity
- Role-based authorization

### InnovationLab.Landing

- Landing page content management
- Public-facing API endpoints
- General platform information

### InnovationLab.Learn

- Learning resource management
- Educational content APIs
- Course and tutorial handling

## Troubleshooting

### Common Issues

1. **Database Connection Issues**

   - Verify PostgreSQL is running
   - Check connection string in `appsettings.json`
   - Ensure database and user exist

2. **Port Conflicts**

   - Check if ports 5001, 5002, 5003 are available
   - Modify port numbers in `compose.yml` or `launchSettings.json` if needed

3. **Migration Errors**

   - Ensure database is accessible
   - Check Entity Framework CLI is installed: `dotnet tool install --global dotnet-ef`

4. **Docker Issues**
   - Ensure Docker Desktop is running
   - Check `.env` file exists and has correct values
   - Verify no port conflicts with existing services

### Logs and Debugging

**For .NET CLI:**

```bash
# Run with detailed logging
dotnet run --environment Development
```

**For Docker:**

```bash
# View logs for all services
docker compose logs

# View logs for specific service
docker compose logs auth-api
docker compose logs landing-api
docker compose logs learn-api
```

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct and the process for submitting pull requests.

### Development Workflow

1. Create a feature branch: `git checkout -b feature/your-feature`
2. Make your changes
3. Follow conventional commit messages
4. Push to your branch: `git push origin feature/your-feature`
5. Create a Pull Request from GitHub

## License

This project is not licensed yet.

## Support

If you encounter any issues or need help:

1. Check the [Troubleshooting](#troubleshooting) section
2. Review existing GitHub Issues
3. Create a new issue with detailed information about your problem

---

**Keep Innovating!!**
