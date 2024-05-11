#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 50050
EXPOSE 50051

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["gambling-sport-member/gambling-sport-member.csproj", "gambling-sport-member/"]
RUN dotnet restore "gambling-sport-member/gambling-sport-member.csproj"
COPY . .
WORKDIR "/src/gambling-sport-member"
RUN dotnet build "gambling-sport-member.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "gambling-sport-member.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gambling-sport-member.dll"]
