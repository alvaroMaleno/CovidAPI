FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8085

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Covid_REST.csproj", "./"]
RUN dotnet restore "Covid_REST.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Covid_REST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Covid_REST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Covid_REST.dll"]
