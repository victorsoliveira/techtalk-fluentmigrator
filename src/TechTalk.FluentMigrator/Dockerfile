#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/TechTalk.FluentMigrator/TechTalk.FluentMigrator.Api.csproj", "src/TechTalk.FluentMigrator/"]
COPY ["src/TechTalk.FluentMigrator.Domain/TechTalk.FluentMigrator.Domain.csproj", "src/TechTalk.FluentMigrator.Domain/"]
COPY ["src/TechTalk.FluentMigrator.Infra/TechTalk.FluentMigrator.Infra.csproj", "src/TechTalk.FluentMigrator.Infra/"]
RUN dotnet restore "src/TechTalk.FluentMigrator/TechTalk.FluentMigrator.Api.csproj"
COPY . .
WORKDIR "/src/src/TechTalk.FluentMigrator"
RUN dotnet build "TechTalk.FluentMigrator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechTalk.FluentMigrator.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechTalk.FluentMigrator.Api.dll"]