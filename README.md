# Game Store API

## Starting SQL Server

```bash
export sa_password="[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -v gamestoredb:/var/opt/mssql -d --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```

## Setting the connection string to secret manager

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost; Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True"
```

## Setting up Migrations

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
dotnet ef database update
```

## Create access token for local development
  
```bash
dotnet user-jwts create
```

## Print access token details for local development

```bash
dotnet user-jwts print id_here
```

## JSON Logger
```csharp
builder.Logging.AddJsonConsole(options => options.JsonWriterOptions = new() { Indented = true });
```