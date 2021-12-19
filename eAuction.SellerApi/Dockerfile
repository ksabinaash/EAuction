#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eAuction.SellerApi/eAuction.SellerApi.csproj", "eAuction.SellerApi/"]
RUN dotnet restore "eAuction.SellerApi/eAuction.SellerApi.csproj"
COPY . .
WORKDIR "/src/eAuction.SellerApi"
RUN dotnet build "eAuction.SellerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAuction.SellerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAuction.SellerApi.dll"]