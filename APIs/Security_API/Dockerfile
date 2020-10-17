FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8082

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["API_Security.csproj", "./"]
RUN dotnet restore "API_Security.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "API_Security.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_Security.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API_Security.dll"]
