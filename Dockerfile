# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# copy csproj trước để cache restore
COPY ToyotaWeb.csproj ./
RUN dotnet restore

# copy toàn bộ source
COPY . .

# publish project
RUN dotnet publish ToyotaWeb.csproj -c Release -o /app/publish /p:UseAppHost=false


# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

# copy build result
COPY --from=build /app/publish .

# port render dùng
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# run app
ENTRYPOINT ["dotnet", "ToyotaWeb.dll"]
