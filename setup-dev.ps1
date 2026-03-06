$ErrorActionPreference = 'Stop'

$ProjectRoot = $PSScriptRoot

Write-Host "Setting up local environment..." -ForegroundColor Green

Set-Location -Path $ProjectRoot

if (-Not (Test-Path ".env")) {
    Copy-Item ".env.example" -Destination ".env"
    Write-Host "Created .env file from template." -ForegroundColor Yellow
}

if (Test-Path ".env") {
    Get-Content ".env" | Where-Object { $_ -match '=' -and $_ -notmatch '^#' } | ForEach-Object {
        $parts = $_ -split '=', 2
        $name = $parts[0].Trim()
        $value = $parts[1].Trim()

        if ($value -match '^"(.*)"$') {
            $value = $Matches[1] 
        } elseif ($value -match "^'(.*)'$") {
            $value = $Matches[1] 
        }

        Set-Item -Path "Env:$name" -Value $value
    }
    Write-Host "Environment variables loaded from .env"
} else {
    Write-Error "Error: .env file not found."
    exit 1
}

Write-Host "----------------------------------------"

function Setup-Service {
    param (
        [string]$ServicePath,
        [hashtable]$Secrets
    )

    Write-Host "Configuring $ServicePath..."

    Push-Location "$ProjectRoot/$ServicePath"

    try {
        dotnet user-secrets init 2>$null | Out-Null

        foreach ($key in $Secrets.Keys) {
            $value = $Secrets[$key]
            dotnet user-secrets set $key $value | Out-Null
        }

        Write-Host "✓ Secrets configured successfully." -ForegroundColor Green
    } finally {

        Pop-Location
    }
}

Setup-Service -ServicePath "backend/src/Backend/Echoes.API" -Secrets @{
    "ConnectionStrings:DefaultConnection" = "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$env:POSTGRES_PASSWORD"
    "ConnectionStrings:RedisPubSub"        = "localhost:6379,password=$env:REDIS_PUBSUB_PASSWORD"
    "JwtSettings:Secret"                  = $env:JWT_SECRET
}

Setup-Service -ServicePath "backend/src/Backend/Echoes.GameServer" -Secrets @{
    "ConnectionStrings:RedisPubSub"   = "localhost:6379,password=$env:REDIS_PUBSUB_PASSWORD"
    "ConnectionStrings:RedisBuffer" = "localhost:6380,password=$env:REDIS_BUFFER_PASSWORD"
}

Setup-Service -ServicePath "backend/src/Backend/Echoes.PersistenceWorker" -Secrets @{
    "ConnectionStrings:DefaultConnection" = "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$env:POSTGRES_PASSWORD"
    "ConnectionStrings:RedisBuffer"        = "localhost:6380,password=$env:REDIS_BUFFER_PASSWORD"
}

Setup-Service -ServicePath "backend/src/Backend/Echoes.CryptoWorker" -Secrets @{
    "ConnectionStrings:DefaultConnection" = "Host=localhost;Database=echoes_bathala_db;Username=admin;Password=$env:POSTGRES_PASSWORD"
}

Write-Host "----------------------------------------"
Write-Host "Setup complete! You can now run 'docker-compose up -d'." -ForegroundColor Green
