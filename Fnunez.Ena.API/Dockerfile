FROM mcr.microsoft.com/dotnet/aspnet:6.0.16-alpine3.17-amd64 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0.408-alpine3.17-amd64 AS build
WORKDIR /app
COPY ./ ./ 

RUN echo 'Building API container'

WORKDIR /app/Fnunez.Ena.API

RUN dotnet restore

RUN dotnet build "Fnunez.Ena.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fnunez.Ena.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Fnunez.Ena.API.dll"]