# Dockerfile located at: KindQuestAPI/KindQuestAPI/Dockerfile
# Build context is repository root

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file first (use path relative to repo root)
COPY ["KindQuestAPI/KindQuestAPI/KindQuestAPI.csproj", "KindQuestAPI/KindQuestAPI/"]

# Set WORKDIR to the project directory within /src
WORKDIR /src/KindQuestAPI/KindQuestAPI
# Restore packages for the specific project
RUN dotnet restore "KindQuestAPI.csproj"

# Copy the rest of the project's source code
# Go back to /src to ensure the relative source path is correct
WORKDIR /src
COPY ["KindQuestAPI/KindQuestAPI/", "KindQuestAPI/KindQuestAPI/"]

# Set WORKDIR back to the project directory for build
WORKDIR /src/KindQuestAPI/KindQuestAPI
RUN dotnet build "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited from the previous stage (/src/KindQuestAPI/KindQuestAPI)
RUN dotnet publish "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
# Ensure the DLL name matches your project's output assembly name
ENTRYPOINT ["dotnet", "KindQuestAPI.dll"]