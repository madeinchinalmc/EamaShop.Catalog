FROM microsoft/aspnetcore:2.0.3 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY *.sln ./
COPY src/EamaShop.Catalog.API/EamaShop.Catalog.API.csproj src/EamaShop.Catalog.API/
RUN dotnet restore
COPY . .
WORKDIR /src/src/EamaShop.Catalog.API
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EamaShop.Catalog.API.dll"]
