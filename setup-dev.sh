#!/bin/bash

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${GREEN}Setting up local environment...${NC}"

if [ ! -f .env ]; then
	cp .env.example .env
	echo -e "${YELLOW}Created .env file from template.${NC}"
fi

if [ -f .env ]; then
	export "$(grep -v '^#' .env | xargs)"
	echo "Environment variables loaded from .env"
fi

echo "Configuring Echoes.API..."
cd backend/src/Backend/Echoes.API || exit
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
dotnet user-secrets set "Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD"
dotnet user-secrets set "JwtSettings:Secret" "$JWT_SECRET"
cd ../../../../

echo "Configuring Echoes.GameServer..."
cd backend/src/Backend/Echoes.GameServer || exit
dotnet user-secrets init
dotnet user-secrets set "Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD"
dotnet user-secrets set "Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"
cd ../../../../

echo "Configuring Echoes.PersistenceWorker..."
cd backend/src/Backend/Echoes.PersistenceWorker || exit
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
dotnet user-secrets set "Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"
cd ../../../../

echo "Configuring Echoes.CryptoWorker..."
cd backend/src/Backend/Echoes.CryptoWorker || exit
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
Set-Location ../../../../
cd ../../../../

echo -e "${GREEN}Setup complete! You can now run 'docker-compose up -d'.${NC}"
