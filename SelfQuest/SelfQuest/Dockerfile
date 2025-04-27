# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

# Set workdir early
WORKDIR /src

# *** START: Install Node.js and ENABLE COREPACK (for Yarn) ***
USER root
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs && \
    # Enable Corepack to manage Yarn
    corepack enable && \
    # Clean up apt lists
    rm -rf /var/lib/apt/lists/*
# *** END: Install Node.js and Corepack ***

# Switch back to the non-root container user 'app'
USER app

# Ensure 'app' user owns the working directory
USER root
RUN chown -R app:app /src
USER app

# Copy project file first for layer caching
COPY ["SelfQuest.csproj", "."]
RUN dotnet restore "SelfQuest.csproj"

# Copy the rest of the source code, including package.json and yarn.lock
COPY [".", "."]

# *** Use Yarn to install dependencies ***
# Use --frozen-lockfile in CI/CD to ensure lockfile isn't changed
RUN yarn install --frozen-lockfile

# Build the project (runs dotnet build, which triggers npm/yarn scripts from csproj)
RUN dotnet build "SelfQuest.csproj" -c $BUILD_CONFIGURATION --no-restore -v diag

# Stage 2: Publish (Likely unchanged)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet publish "SelfQuest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --no-build

# Stage 3: Final runtime image (Likely unchanged)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SelfQuest.dll"]