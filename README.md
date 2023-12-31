# Game Store API
A .NET Rest API for a game store using Minimal APIs, Entity Framework Core, and SQL Server.

Features include:
- Authentication and Authorization using JWT Bearer Tokens, OAuth2.0, OpenID Connect using Auth0
- Repository pattern for data access
- Microsoft SQL Server integration using Docker
- Entity Framework Core integration for data access and migrations
- Logging, error handling and middleware
- API versioning
- Pagination and search
- Field filtering
- API documentation using Swagger and Postman
- Cloudinary integration for image storage
- App containerization using Docker and Docker Compose

## Postman API Documentation
[![Run in Postman](https://run.pstmn.io/button.svg)](https://documenter.getpostman.com/view/26440641/2s93zE41DR)

## Starting SQL Server

```bash
export sa_password="[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -v gamestoredb:/var/opt/mssql -d --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```

## Setting the connection string to secret manager

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost; Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True" # for local development
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=db; Database=GameStore; User Id=sa; Password=$sa_password; TrustServerCertificate=True" # for docker
```

## Setting up Migrations

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --output-dir Data/Migrations
dotnet ef database update
```

## Create access token for local development

```bash
dotnet user-jwts create --role "Admin" --scope "games:write"
```

## Print access token details for local development

```bash
dotnet user-jwts print id_here
```

## JSON Logger
```csharp
builder.Logging.AddJsonConsole(options => options.JsonWriterOptions = new() { Indented = true });
```

## Cloudinary Configuration
```bash
dotnet user-secrets set "Cloudinary:CloudName" "cloud_name_here"
dotnet user-secrets set "Cloudinary:ApiKey" "api_key_here"
dotnet user-secrets set "Cloudinary:ApiSecret" "api_secret_here"
```