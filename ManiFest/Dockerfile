FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5130 
ENV ASPNETCORE_URLS=http://+:5130

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

FROM build AS publish
RUN dotnet publish "ManiFest.WebAPI/ManiFest.WebAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
COPY ManiFest.WebAPI/Assets ./Assets

ENTRYPOINT ["dotnet", "ManiFest.WebAPI.dll"]