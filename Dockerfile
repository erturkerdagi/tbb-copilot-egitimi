# Erdağı Bank Hesap API — çok aşamalı (multi-stage) Docker image

# 1. Aşama: derleme (SDK image'ı)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Önce yalnızca proje dosyaları: kod değişse bile paket katmanı önbellekten gelir
COPY global.json ./
COPY src/Bank.Api/Bank.Api.csproj src/Bank.Api/
RUN dotnet restore src/Bank.Api/Bank.Api.csproj

COPY src/ src/
RUN dotnet publish src/Bank.Api/Bank.Api.csproj -c Release -o /app --no-restore

# 2. Aşama: çalışma (yalnızca ASP.NET Core runtime, SDK yok)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# .NET 8 ve sonrasında container'lar varsayılan olarak 8080 portunu dinler
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ConnectionStrings__BankDb="Data Source=/app/data/erdagibank.db"

# Veritabanı klasörü root olmayan uygulama kullanıcısına ait
RUN mkdir -p /app/data && chown "$APP_UID" /app/data

COPY --from=build /app .

# Microsoft'un .NET image'larındaki hazır, root olmayan kullanıcı
USER $APP_UID

EXPOSE 8080
ENTRYPOINT ["dotnet", "Bank.Api.dll"]
