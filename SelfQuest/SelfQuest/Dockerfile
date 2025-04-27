# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# *** START: Install Node.js and npm ***
# Update package lists and install Node.js (which includes npm) and curl (needed for the NodeSource script)
# Use root user temporarily for installations
USER root
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs
# Switch back to the default container user
USER app
# *** END: Install Node.js and npm ***

# Copy the project file
COPY ["SelfQuest.csproj", "."]

# Restore packages for the project file
RUN dotnet restore "SelfQuest.csproj"

# Copy the rest of the source code
COPY [".", "."]

# *** Install npm packages defined in package.json (if you have one) ***
# This is crucial if 'npm run tw:build:prod' depends on packages like tailwindcss
# Make sure package.json and package-lock.json (if used) are copied by the COPY [".", "."] step above
RUN npm install
# *** ---------------------------------------------------------------- ***

# Build the project (npm should now be available)
RUN dotnet build "SelfQuest.csproj" -c $BUILD_CONFIGURATION --no-restore

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited (/src)
# No need to install npm again here unless publish step also runs npm scripts
RUN dotnet publish "SelfQuest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
# Ensure the DLL name matches your project's output assembly name
ENTRYPOINT ["dotnet", "SelfQuest.dll"]