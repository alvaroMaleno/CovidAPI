FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5006 5005
# ENV ASPNETCORE_URLS=http://*:5005

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["API_DAO.csproj", "./"]
RUN dotnet restore "API_DAO.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "API_DAO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API_DAO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "API_DAO.dll"]
CMD ASPNETCORE_URLS=http://*:5005 dotnet API_DAO.dll
