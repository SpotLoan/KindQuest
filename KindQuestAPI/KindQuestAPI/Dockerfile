# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# +++ DIAGNOSTIC STEP +++
# List the contents of the workspace directory recursively
# This will show us the actual file structure checked out by Cloud Build
RUN echo "--- Listing /workspace contents ---" && ls -R /workspace && echo "--- End of listing ---"
# +++++++++++++++++++++++

# This COPY command is still expected to fail, but the log above will help
COPY ["KindQuest/KindQuestAPI/KindQuestAPI/KindQuestAPI.csproj", "KindQuest/KindQuestAPI/KindQuestAPI/"]

WORKDIR "/src/KindQuest/KindQuestAPI/KindQuestAPI"
RUN dotnet restore "KindQuestAPI.csproj"

WORKDIR /src
COPY ["KindQuest/KindQuestAPI/KindQuestAPI/", "KindQuest/KindQuestAPI/KindQuestAPI/"]

WORKDIR "/src/KindQuest/KindQuestAPI/KindQuestAPI"
RUN dotnet build "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KindQuestAPI.dll"]