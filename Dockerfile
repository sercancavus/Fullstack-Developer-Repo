# 1. Adım: Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Proje dosyalarını kopyala
COPY . ./
# Bağımlılıkları yükle (Restore)
RUN dotnet restore

# Uygulamayı derle (Publish)
RUN dotnet publish -c Release -o out

# 2. Adım: Çalıştırma aşaması
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Buraya projenin dll ismini yazmalısın (Örn: ProjeAdi.dll)
ENTRYPOINT ["dotnet", "SeninProjeAdin.dll"]
