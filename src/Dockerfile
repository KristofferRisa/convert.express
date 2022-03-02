FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY Convert.Express.csproj .
RUN dotnet restore Convert.Express.csproj
COPY . .
RUN dotnet build Convert.Express.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Convert.Express.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Convert.Express.dll"]
