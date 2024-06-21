FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GreenMileSharing.TripApi/GreenMileSharing.TripApi.csproj", "GreenMileSharing.TripApi/"]
COPY ["GreenMileSharing.Domain/GreenMileSharing.Domain.csproj", "GreenMileSharing.Domain/"]
COPY ["GreenMileSharing.Messages/GreenMileSharing.Messages.csproj", "GreenMileSharing.Messages/"]
COPY ["GreenMileSharing.Shared/GreenMileSharing.Shared.csproj", "GreenMileSharing.Shared/"]
RUN dotnet restore "GreenMileSharing.TripApi/GreenMileSharing.TripApi.csproj"
COPY . .
WORKDIR "/src/GreenMileSharing.TripApi"
RUN dotnet build "GreenMileSharing.TripApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "GreenMileSharing.TripApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenMileSharing.TripApi.dll"]
