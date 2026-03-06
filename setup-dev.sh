#!/bin/bash

set -e

GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" >/dev/null 2>&1 && pwd)"
PROJECT_ROOT="$SCRIPT_DIR"

cd "$PROJECT_ROOT" || exit 1

echo -e "${GREEN}Setting up local environment...${NC}"

if [ ! -f .env ]; then
	cp .env.example .env
	echo -e "${YELLOW}Created .env file from template.${NC}"
fi

if [ -f .env ]; then
	set -a
	source .env
	set +a
	echo "Environment variables loaded from .env"
else
	echo "Error: .env file not found."
	exit 1
fi

echo "----------------------------------------"

setup_service() {
	local service_path=$1
	echo "Configuring $service_path..."

	(
		cd "$PROJECT_ROOT/$service_path" || exit 1

		dotnet user-secrets init >/dev/null 2>&1

		shift

		while [ $# -gt 0 ]; do
			local key=$1
			local value=$2

			dotnet user-secrets set "$key" "$value" >/dev/null

			shift 2
		done

		echo -e "${GREEN}✓ Secrets configured successfully.${NC}"
	)
}

setup_service "backend/src/Backend/Echoes.API" \
	"ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD" \
	"Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD" \
	"JwtSettings:Secret" "$JWT_SECRET"

setup_service "backend/src/Backend/Echoes.GameServer" \
	"Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD" \
	"Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"

setup_service "backend/src/Backend/Echoes.PersistenceWorker" \
	"ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD" \
	"Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"

setup_service "backend/src/Backend/Echoes.CryptoWorker" \
	"ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"

echo "----------------------------------------"
echo -e "${GREEN}Setup complete! You can now run 'docker-compose up -d'.${NC}"
