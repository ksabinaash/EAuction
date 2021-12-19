#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eAuction.BuyerApi/eAuction.BuyerApi.csproj", "eAuction.BuyerApi/"]
RUN dotnet restore "eAuction.BuyerApi/eAuction.BuyerApi.csproj"
COPY . .
WORKDIR "/src/eAuction.BuyerApi"
RUN dotnet build "eAuction.BuyerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.BuyerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.BuyerApi.dll"]