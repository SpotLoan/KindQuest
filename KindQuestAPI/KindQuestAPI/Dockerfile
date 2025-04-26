# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# --- CORRECTED PATH (Including top-level 'KindQuest' folder) ---
COPY ["KindQuest/KindQuestAPI/KindQuestAPI/KindQuestAPI.csproj", "KindQuest/KindQuestAPI/KindQuestAPI/"]

# --- CORRECTED PATH ---
WORKDIR "/src/KindQuest/KindQuestAPI/KindQuestAPI"
RUN dotnet restore "KindQuestAPI.csproj"

# Copy the rest of the application source code
WORKDIR /src
# --- CORRECTED PATH ---
# Copy the entire nested project directory content
COPY ["KindQuest/KindQuestAPI/KindQuestAPI/", "KindQuest/KindQuestAPI/KindQuestAPI/"]
# (Optional: If your .sln file is at KindQuest/KindQuestAPI/KindQuest.sln, you might copy it too)
# COPY ["KindQuest/KindQuestAPI/YourSolution.sln", "KindQuest/KindQuestAPI/"] # Adjust as needed

# --- CORRECTED PATH ---
WORKDIR "/src/KindQuest/KindQuestAPI/KindQuestAPI"
RUN dotnet build "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited: /src/KindQuest/KindQuestAPI/KindQuestAPI
# --- CORRECTED PATH ---
RUN dotnet publish "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KindQuestAPI.dll"]