FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5130 
ENV ASPNETCORE_URLS=http://+:5130

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish "CallTaxi.WebAPI/CallTaxi.WebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY CallTaxi.WebAPI/Assets ./Assets

ENTRYPOINT ["dotnet", "CallTaxi.WebAPI.dll"]