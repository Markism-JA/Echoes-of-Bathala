FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Backend/Echoes.API/Echoes.API.csproj", "src/Backend/Echoes.API/"]
COPY ["src/Backend/Echoes.Application/Echoes.Application.csproj", "src/Backend/Echoes.Application/"]
COPY ["src/Backend/Echoes.Domain/Echoes.Domain.csproj", "src/Backend/Echoes.Domain/"]
COPY ["src/Backend/Echoes.Infrastructure/Echoes.Infrastructure.csproj", "src/Backend/Echoes.Infrastructure/"]
COPY ["src/Shared/Echoes.Shared.Abstraction/Echoes.Shared.Abstraction.csproj", "src/Shared/Echoes.Shared.Abstraction/"]
COPY ["src/Shared/Echoes.Shared.Domain/Echoes.Shared.Domain.csproj", "src/Shared/Echoes.Shared.Domain/"]
COPY ["src/Shared/Echoes.Shared.Network/Echoes.Shared.Network.csproj", "src/Shared/Echoes.Shared.Network/"]

RUN dotnet restore "src/Backend/Echoes.API/Echoes.API.csproj"

COPY src/ ./src/

WORKDIR "/src/src/Backend/Echoes.API"
RUN dotnet build "Echoes.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Echoes.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Echoes.API.dll"]
