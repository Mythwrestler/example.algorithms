FROM registry.casperinc.net/dotnet/sdk:5.0-focal AS build-env

WORKDIR /app

# Copy in project files
COPY NuGet.Config ./
COPY *.csproj ./

# Restore Nuget Packages
RUN dotnet restore

# Copy everything else and build
COPY . ./

# RUN dotnet publish -c Publish -o out 
RUN dotnet publish -c Release -o out

FROM registry.casperinc.net/dotnet/aspnet:5.0-focal

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Example.Algorithms.Utility.dll"]