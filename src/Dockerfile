#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ODF.API/ODF.API.csproj", "ODF.API/"]
COPY ["ODF.AppLayer/ODF.AppLayer.csproj", "ODF.AppLayer/"]
COPY ["ODF.DataContracts/ODF.Data.Contracts.csproj", "ODF.DataContracts/"]
COPY ["ODF.Enums/ODF.Enums.csproj", "ODF.Enums/"]
COPY ["ODF.Data.Elastic/ODF.Data.Elastic.csproj", "ODF.Data.Elastic/"]
COPY ["ODF.Data.Redis/ODF.Data.Redis.csproj", "ODF.Data.Redis/"]
COPY ["ODF.ServiceLayer/ODF.ServiceLayer.csproj", "ODF.ServiceLayer/"]
RUN dotnet restore "ODF.API/ODF.API.csproj"

COPY . .
WORKDIR "/src/ODF.API"
RUN dotnet build "ODF.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ODF.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ODF.API.dll"]