#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eAuction.ServiceBusClient/eAuction.ServiceBusClient.csproj", "eAuction.ServiceBusClient/"]
RUN dotnet restore "eAuction.ServiceBusClient/eAuction.ServiceBusClient.csproj"
COPY . .
WORKDIR "/src/eAuction.ServiceBusClient"
RUN dotnet build "eAuction.ServiceBusClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.ServiceBusClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.ServiceBusClient.dll"]