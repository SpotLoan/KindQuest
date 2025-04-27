# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Set workdir early (will be owned by default user, likely 'app')
WORKDIR /src

# *** START: Install Node.js and npm ***
# Switch to root user for installations
USER root
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs && \
    # Clean up apt lists to reduce image size
    rm -rf /var/lib/apt/lists/*
# *** END: Install Node.js and npm ***

# Switch back to the non-root container user 'app'
USER app

# *** START: Ensure 'app' user owns the working directory ***
# This step might not be strictly necessary if WORKDIR was created by 'app' initially,
# but it explicitly ensures the correct ownership after switching back from root.
# We temporarily switch back to root to run chown, then back to app.
USER root
RUN chown -R app:app /src
USER app
# *** END: Ensure 'app' user owns the working directory ***

# Now, copy files as the 'app' user into the directory it owns
COPY ["SelfQuest.csproj", "."]
RUN dotnet restore "SelfQuest.csproj"

# Copy the rest of the source code
COPY [".", "."]

# Install npm packages (ensure package.json is copied in the previous step)
RUN npm install

# Build the project (also runs as 'app' user)
RUN dotnet build "SelfQuest.csproj" -c $BUILD_CONFIGURATION --no-restore

# Stage 2: Publish (unchanged from previous)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited (/src)
RUN dotnet publish "SelfQuest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Final runtime image (unchanged from previous)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SelfQuest.dll"]