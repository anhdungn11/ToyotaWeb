FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
COPY ToyotaWeb.csproj ./
RUN dotnet restore
COPY . .

RUN dotnet publish ToyotaWeb.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE 8080
ENTRYPOINT ["dotnet", "ToyotaWeb.dll"]
