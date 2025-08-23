# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY InnovationLab.Auth/InnovationLab.Auth.csproj InnovationLab.Auth/
COPY InnovationLab.Shared/InnovationLab.Shared.csproj InnovationLab.Shared/
RUN dotnet restore InnovationLab.Auth/InnovationLab.Auth.csproj

# Copy the rest and publish
COPY . .
RUN dotnet publish InnovationLab.Auth/InnovationLab.Auth.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InnovationLab.Auth.dll"]
