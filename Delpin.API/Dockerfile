#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Delpin.API/Delpin.API.csproj", "Delpin.API/"]
COPY ["Delpin.Infrastructure/Delpin.Infrastructure.csproj", "Delpin.Infrastructure/"]
COPY ["Delpin.Domain/Delpin.Domain.csproj", "Delpin.Domain/"]
COPY ["Delpin.Application/Delpin.Application.csproj", "Delpin.Application/"]
RUN dotnet restore "Delpin.API/Delpin.API.csproj"
COPY . .
WORKDIR "/src/Delpin.API"
RUN dotnet build "Delpin.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Delpin.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delpin.API.dll"]
