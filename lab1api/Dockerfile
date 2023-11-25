#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["lab1api/lab1api.csproj", "lab1api/"]
RUN dotnet restore "lab1api/lab1api.csproj"
COPY . .
WORKDIR "/src/lab1api"
RUN dotnet build "lab1api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "lab1api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "lab1api.dll"]