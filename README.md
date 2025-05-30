## Running the Project with Docker

This project is containerized using Docker and Docker Compose, providing a multi-service environment including the main .NET 8.0 application, MongoDB, PostgreSQL, and RabbitMQ. Below are the project-specific instructions and requirements for running the application in Docker.

### Project-Specific Requirements
- **.NET Version:** The application targets **.NET 8.0** (as set in the Dockerfile `ARG DOTNET_VERSION=8.0`).
- **Dependencies:**
  - MongoDB
  - PostgreSQL
  - RabbitMQ
- **Firebase Credentials:** The file `ProductMS.Infrastructure/firebase-credentials.json` is required at runtime and is copied into the container.

### Environment Variables
- **PostgreSQL:**
  - `POSTGRES_USER=productms`
  - `POSTGRES_PASSWORD=productms`
  - `POSTGRES_DB=productms`
- **No additional environment variables are required by default for the main application.**
- **Optional:** You may use a `.env` file for the main service by uncommenting the `env_file` line in `docker-compose.yml` if needed.

### Build and Run Instructions
1. **Ensure Docker and Docker Compose are installed.**
2. **From the project root, run:**
   ```sh
   docker compose up --build
   ```
   This will build the .NET application and start all services defined in `docker-compose.yml`.

### Special Configuration
- **Database Initialization:**
  - The `init_db` directory is mounted into the PostgreSQL container for initial schema/data setup.
- **Firebase Credentials:**
  - The `firebase-credentials.json` file is required and is automatically copied into the application container.
- **User Permissions:**
  - The application runs as a non-root user inside the container for improved security.

### Exposed Ports
- **csharp-productms (ASP.NET Core app):**
  - `8080:8080` (host:container)
- **MongoDB:**
  - `27017:27017`
- **PostgreSQL:**
  - `5432:5432`
- **RabbitMQ:**
  - `5672:5672` (AMQP protocol)
  - `15672:15672` (Management UI)

### Notes
- If you need to change the exposed port for the main application, adjust the `ports` section in `docker-compose.yml` accordingly.
- Healthchecks are configured for all services to ensure proper startup and readiness.
- All services are connected via the `backend` Docker network.

---

_If you have additional configuration needs (e.g., custom environment variables, different database credentials), update the `docker-compose.yml` and/or provide a `.env` file as appropriate._