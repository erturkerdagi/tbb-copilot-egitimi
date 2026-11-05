FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Bank.Api.csproj ./
RUN dotnet restore Bank.Api.csproj

COPY . ./
RUN dotnet publish Bank.Api.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./
RUN mkdir -p /app/data && chown -R 1654:1654 /app/data

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ConnectionStrings__BankDb="Data Source=/app/data/erdagibank.db"
EXPOSE 8080

USER 1654
ENTRYPOINT ["dotnet", "Bank.Api.dll"]