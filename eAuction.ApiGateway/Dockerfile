#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["eAucation.ApiGateway/eAucation.ApiGateway.csproj", "eAucation.ApiGateway/"]
RUN dotnet restore "eAucation.ApiGateway/eAucation.ApiGateway.csproj"
COPY . .
WORKDIR "/src/eAucation.ApiGateway"
RUN dotnet build "eAucation.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eAucation.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eAucation.ApiGateway.dll"]