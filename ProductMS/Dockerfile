# syntax=docker/dockerfile:1

# Set the .NET version from the csproj (net8.0)
ARG DOTNET_VERSION=8.0

# --- Build Stage ---
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build
WORKDIR /src

# Copy only the project files for restore (improves cache usage)
COPY --link ProductMS/ProductMS.csproj ProductMS/
COPY --link ProductMS.Infrastructure/ProductMS.Infrastructure.csproj ProductMS.Infrastructure/
COPY --link ProductMS.Application/ProductMS.Application.csproj ProductMS.Application/
COPY --link ProductMS.Core/ProductMS.Core.csproj ProductMS.Core/
COPY --link ProductMS.Commons/ProductMS.Commons.csproj ProductMS.Commons/
COPY --link ProductMS.Domain/ProductMS.Domain.csproj ProductMS.Domain/

# Restore dependencies with cache mounts for nuget and msbuild
RUN --mount=type=cache,target=/root/.nuget/packages \
    --mount=type=cache,target=/root/.cache/msbuild \
    dotnet restore ProductMS/ProductMS.csproj

# Copy the rest of the source code
COPY --link ProductMS/ ProductMS/
COPY --link ProductMS.Infrastructure/ ProductMS.Infrastructure/
COPY --link ProductMS.Application/ ProductMS.Application/
COPY --link ProductMS.Core/ ProductMS.Core/
COPY --link ProductMS.Commons/ ProductMS.Commons/
COPY --link ProductMS.Domain/ ProductMS.Domain/

# Build and publish the application
WORKDIR /src/ProductMS
RUN dotnet build ProductMS.csproj -c Release -o /app/build
RUN dotnet publish ProductMS.csproj -c Release -o /app/publish /p:UseAppHost=false

# --- Runtime Stage ---
FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION} AS final
WORKDIR /app

# Create a non-root user for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Copy published output from build stage
COPY --from=build /app/publish .
# Copy Firebase credentials (needed at runtime)
COPY --from=build /src/ProductMS.Infrastructure/firebase-credentials.json /app/firebase-credentials.json

# Set permissions for the non-root user
RUN chown -R appuser:appgroup /app
USER appuser

ENTRYPOINT ["dotnet", "ProductMS.dll"]
