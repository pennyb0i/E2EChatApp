FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["E2EChatApp/E2EChatApp.csproj", "E2EChatApp/"]
RUN dotnet restore "E2EChatApp/E2EChatApp.csproj"
COPY . .
WORKDIR "/src/E2EChatApp"
RUN dotnet build "E2EChatApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "E2EChatApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "E2EChatApp.dll"]
