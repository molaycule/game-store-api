# Use the official .NET 7.0 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the project files to the container
COPY . ./

# Restore the NuGet packages
RUN dotnet restore

# Publish the application
RUN dotnet publish -c Release -o out

# Create a new image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out ./

# Expose the desired port (replace <PORT_NUMBER> with the actual port number used in your app)
EXPOSE 80

# Set the entry point for the container
ENTRYPOINT ["dotnet", "GameStore.Api.dll"]
