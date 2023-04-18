#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.hamdocker.ir/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.hamdocker.ir/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vekalat.UI/Vekalat.UI.csproj", "Vekalat.UI/"]
COPY ["Vekalat.InfraStructure.Data/Vekalat.InfraStructure.Data.csproj", "Vekalat.InfraStructure.Data/"]
COPY ["Vekalat.Application/Vekalat.Application.csproj", "Vekalat.Application/"]
COPY ["Vekalat.Core/Vekalat.Core.csproj", "Vekalat.Core/"]
COPY ["Fiver.Api.HttpClient/Fiver.Api.HttpClient.csproj", "Fiver.Api.HttpClient/"]
COPY ["Vekalat.Infrastructure.Data.Migrations/Vekalat.Infrastructure.Data.Migrations.csproj", "Vekalat.Infrastructure.Data.Migrations/"]
COPY ["Vekalat.InfraStructure.Persistant/Vekalat.InfraStructure.Persistant.csproj", "Vekalat.InfraStructure.Persistant/"]
RUN dotnet restore "Vekalat.UI/Vekalat.UI.csproj"
COPY . .
WORKDIR "/src/Vekalat.UI"
RUN dotnet build "Vekalat.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vekalat.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vekalat.UI.dll"]