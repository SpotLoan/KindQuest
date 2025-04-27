# Dockerfile located at: SelfQuest/SelfQuest/Dockerfile
# Build context IS SelfQuest/SelfQuest/ (due to Cloud Build trigger setting)

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file (it's at the root of the build context)
COPY ["SelfQuest.csproj", "."]

# Restore packages for the project file copied to WORKDIR (/src)
RUN dotnet restore "SelfQuest.csproj"

# Copy the rest of the source code from the build context root
# (e.g., Program.cs, appsettings.json, etc.) into WORKDIR (/src)
COPY [".", "."]

# Build the project (already in the correct WORKDIR: /src)
RUN dotnet build "SelfQuest.csproj" -c $BUILD_CONFIGURATION --no-restore

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited (/src)
RUN dotnet publish "SelfQuest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
# Ensure the DLL name matches your project's output assembly name
ENTRYPOINT ["dotnet", "SelfQuest.dll"]
