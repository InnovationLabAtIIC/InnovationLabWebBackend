# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY InnovationLab.Landing/InnovationLab.Landing.csproj InnovationLab.Landing/
COPY InnovationLab.Shared/InnovationLab.Shared.csproj InnovationLab.Shared/
RUN dotnet restore InnovationLab.Landing/InnovationLab.Landing.csproj

# Copy the rest and publish
COPY . .
RUN dotnet publish InnovationLab.Landing/InnovationLab.Landing.csproj -c Release -o /app/publish --no-restore

# Stage 2: Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "InnovationLab.Landing.dll"]
