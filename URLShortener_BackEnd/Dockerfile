# Use the official .NET SDK as a base image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the solution and project files
COPY URLShortener_BackEnd.sln ./
COPY URLShortener_BackEnd/ URLShortener_BackEnd/

# Restore dependencies
WORKDIR /app/URLShortener_BackEnd
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o /app/publish

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
EXPOSE 5168
ENTRYPOINT ["dotnet", "URLShortener_BackEnd.dll"]

