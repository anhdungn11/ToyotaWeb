# ================= BUILD STAGE =================
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# copy project file
COPY ToyotaWeb.csproj ./

# restore packages
RUN dotnet restore

# copy toàn bộ source
COPY . .

# publish project
RUN dotnet publish ToyotaWeb.csproj -c Release -o /app/publish /p:UseAppHost=false


# ================= RUNTIME STAGE =================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

# copy file đã publish
COPY --from=build /app/publish .

# render yêu cầu app bind port
ENV ASPNETCORE_URLS=http://+:${PORT}

# expose port
EXPOSE 8080

# start app
ENTRYPOINT ["dotnet", "ToyotaWeb.dll"]
