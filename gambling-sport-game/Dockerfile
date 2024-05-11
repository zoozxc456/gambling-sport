#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 20050
EXPOSE 20051

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gambling-sport-game/gambling-sport-game.csproj", "gambling-sport-game/"]
RUN dotnet restore "gambling-sport-game/gambling-sport-game.csproj"
COPY . .
WORKDIR "/src/gambling-sport-game"
RUN dotnet build "gambling-sport-game.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gambling-sport-game.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gambling-sport-game.dll"]
