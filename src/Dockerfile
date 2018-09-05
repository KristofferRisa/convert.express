FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

ENV ASPNETCORE_URLS http://+:5005
EXPOSE 5005

FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /src
COPY Convert.Express.csproj .
RUN dotnet restore Convert.Express.csproj
COPY . .
#WORKDIR /src/Convert.Express
RUN dotnet build Convert.Express.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Convert.Express.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Convert.Express.dll"]
