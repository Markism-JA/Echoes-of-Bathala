FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/GameBackend.API/GameBackend.API.csproj", "src/GameBackend.API/"]
COPY ["src/GameBackend.Shared/GameBackend.Shared.csproj", "src/GameBackend.Shared/"]
COPY ["src/GameBackend.Core/GameBackend.Core.csproj", "src/GameBackend.Core/"]
COPY ["src/GameBackend.Infra/GameBackend.Infra.csproj", "src/GameBackend.Infra/"]
COPY ["src/GameBackend.Worker/GameBackend.Worker.csproj", "src/GameBackend.Worker/"]

RUN dotnet restore "src/GameBackend.API/GameBackend.API.csproj"

COPY . .

RUN dotnet publish "src/GameBackend.API/GameBackend.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GameBackend.API.dll"]
