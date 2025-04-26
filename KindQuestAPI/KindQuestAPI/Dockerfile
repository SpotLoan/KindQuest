# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# USER $APP_UID # Consider if you need this user setup in the final image
WORKDIR /app
EXPOSE 8080
# EXPOSE 8081 # Only if your app actually listens on 8081 as well

# Use the .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project file first to leverage Docker layer caching
# Uses the correct nested path from your repository
COPY ["KindQuest/KindQuestAPI/KindQuestAPI/KindQuestAPI.csproj", "KindQuestAPI/KindQuestAPI/"]

# Set the working directory to where the .csproj was copied
WORKDIR "/src/KindQuestAPI/KindQuestAPI"
# Restore dependencies using the project file in the current directory
RUN dotnet restore "KindQuestAPI.csproj"

# Copy the rest of the application source code
# Go back to /src before copying to maintain the directory structure
WORKDIR /src
# Copy the entire nested project directory content into the corresponding container directory
COPY ["KindQuestAPI/KindQuestAPI/", "KindQuestAPI/KindQuestAPI/"]
# (Optional: If you have solution files or other projects at the root KindQuestAPI level, copy them too)
# COPY ["KindQuestAPI/YourSolution.sln", "KindQuestAPI/"] # Example if needed

# Set the working directory again for build/publish
WORKDIR "/src/KindQuestAPI/KindQuestAPI"
RUN dotnet build "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
# WORKDIR is inherited from the 'build' stage (/src/KindQuestAPI/KindQuestAPI)
RUN dotnet publish "KindQuestAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage/runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Ensure the DLL name here matches the output assembly name (usually the project name)
ENTRYPOINT ["dotnet", "KindQuestAPI.dll"]