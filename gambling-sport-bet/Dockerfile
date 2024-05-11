#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 10050
EXPOSE 10051

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gambling-sport-bet/gambling-sport-bet.csproj", "gambling-sport-bet/"]
RUN dotnet restore "gambling-sport-bet/gambling-sport-bet.csproj"
COPY . .
WORKDIR "/src/gambling-sport-bet"
RUN dotnet build "gambling-sport-bet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gambling-sport-bet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gambling-sport-bet.dll"]
