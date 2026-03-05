FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Backend/Echoes.CryptoWorker/Echoes.CryptoWorker.csproj", "src/Backend/Echoes.CryptoWorker/"]
COPY ["src/Backend/Echoes.Application/Echoes.Application.csproj", "src/Backend/Echoes.Application/"]
COPY ["src/Backend/Echoes.Domain/Echoes.Domain.csproj", "src/Backend/Echoes.Domain/"]
COPY ["src/Backend/Echoes.Infrastructure/Echoes.Infrastructure.csproj", "src/Backend/Echoes.Infrastructure/"]
COPY ["src/Shared/Echoes.Shared.Abstraction/Echoes.Shared.Abstraction.csproj", "src/Shared/Echoes.Shared.Abstraction/"]
COPY ["src/Shared/Echoes.Shared.Domain/Echoes.Shared.Domain.csproj", "src/Shared/Echoes.Shared.Domain/"]
COPY ["src/Shared/Echoes.Shared.Network/Echoes.Shared.Network.csproj", "src/Shared/Echoes.Shared.Network/"]

RUN dotnet restore "src/Backend/Echoes.CryptoWorker/Echoes.CryptoWorker.csproj"

COPY src/ ./src/

WORKDIR "/src/src/Backend/Echoes.CryptoWorker"
RUN dotnet build "Echoes.CryptoWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Echoes.CryptoWorker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Echoes.CryptoWorker.dll"]
