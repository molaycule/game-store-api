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
