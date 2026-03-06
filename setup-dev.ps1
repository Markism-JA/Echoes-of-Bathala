Write-Host "Setting up local environment..." -ForegroundColor Green

if (-Not (Test-Path ".env")) {
    Copy-Item ".env.example" -Destination ".env"
    Write-Host "Created .env file from template." -ForegroundColor Yellow
}

if (Test-Path .env) {
    Get-Content .env | Where-Object { $_ -notmatch '^#' } | ForEach-Object {
        $parts = $_ -split '=', 2
        if ($parts.Count -eq 2) {
            Set-Item -Path "Env:$($parts[0])" -Value $parts[1]
        }
    }
}

Write-Host "Configuring Echoes.API..."
Set-Location backend/src/Backend/Echoes.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
dotnet user-secrets set "Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD"
dotnet user-secrets set "JwtSettings:Secret" "$JWT_SECRET"
Set-Location ../../../../

Write-Host "Configuring Echoes.GameServer..."
Set-Location backend/src/Backend/Echoes.GameServer
dotnet user-secrets init
dotnet user-secrets set "Redis:PubSubConnectionString" "localhost:6379,password=$REDIS_PUBSUB_PASSWORD"
dotnet user-secrets set "Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"
Set-Location ../../../../

Write-Host "Configuring Echoes.PersistenceWorker..."
Set-Location backend/src/Backend/Echoes.PersistenceWorker
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
dotnet user-secrets set "Redis:BufferConnectionString" "localhost:6380,password=$REDIS_BUFFER_PASSWORD"
Set-Location ../../../../

Write-Host "Configuring Echoes.CryptoWorker..."
Set-Location backend/src/Backend/Echoes.CryptoWorker
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$POSTGRES_PASSWORD"
Set-Location ../../../../

Write-Host "Setup complete! You can now run 'docker-compose up -d'." -ForegroundColor Green
