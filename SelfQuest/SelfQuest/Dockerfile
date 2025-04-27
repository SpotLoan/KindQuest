FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

# Install Node.js and enable Corepack (for Yarn)
USER root
RUN apt-get update && \
    apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_lts.x | bash - && \
    apt-get install -y nodejs && \
    corepack enable && \
    rm -rf /var/lib/apt/lists/*

# Make sure 'app' owns the working directory
RUN useradd -m app || true
RUN chown -R app:app /src

USER app

# Copy project file and restore dependencies
COPY ["SelfQuest.csproj", "."]
RUN dotnet restore "SelfQuest.csproj"

# Copy the rest of the source code
COPY [".", "."]

# Install JS dependencies
RUN yarn install --frozen-lockfile

# Build the project
RUN dotnet build "SelfQuest.csproj" -c $BUILD_CONFIGURATION --no-restore -v diag

# Stage 2: Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN dotnet publish "SelfQuest.csproj" -c $BUILD_CONFIGURATION -o /src/publish /p:UseAppHost=false --no-build
RUN ls -l /src/publish

# Stage 3: Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /src/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "SelfQuest.dll"]
