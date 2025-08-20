# ASP.NET Core WebAPI ��renci Bilgi Sistemi

## Proje Amac�
Bu proje, .NET 8 ve ASP.NET Core WebAPI kullan�larak temel bir ��renci Bilgi Sistemi olu�turmay� ama�lar. ��renci ekleme, listeleme, g�ncelleme ve silme i�lemleri API �zerinden yap�labilir. Hem server-side hem de client-side validasyon �rnekleri sunulmu�tur.

## Yap�lanlar
- **WebAPI Projesi:** .NET 8 tabanl� WebAPI projesi olu�turuldu.
- **Model:** `Student` s�n�f� ile ��renci bilgileri ve do�rulama �znitelikleri tan�mland�.
- **Veritaban�:** Entity Framework Core ile SQL Server �zerinde kal�c� veri saklama sa�land�.
- **Repository:** CRUD i�lemleri i�in `StudentRepository` katman� olu�turuldu ve EF Core ile g�ncellendi.
- **Controller:** `StudentsController` ile API �zerinden CRUD i�lemleri sa�land�.
- **Validasyon:** Ad, soyad ve ��renci numaras� i�in server-side validasyonlar eklendi.
- **Client Side:** HTML ve JavaScript ile form �zerinden client-side validasyon ve API ba�lant�s� �rnekleri sunuldu.

## Neler Yap�labilir?
- **Kullan�c� Aray�z�:** Daha geli�mi� bir frontend (React, Angular, Blazor) ile kullan�c� dostu aray�z geli�tirilebilir.
- **Kimlik Do�rulama:** JWT veya OAuth ile kullan�c� yetkilendirme ve kimlik do�rulama eklenebilir.
- **Unit Testler:** API ve repository katmanlar� i�in birim testler yaz�labilir.
- **Veritaban� Geli�tirme:** ��renciye ait dersler, notlar gibi ek tablolar ve ili�kiler eklenebilir.
- **API Dok�mantasyonu:** Swagger/OpenAPI ile API dok�mantasyonu geli�tirilebilir.
- **Deployment:** Proje bulut ortam�na (Azure, AWS) veya Docker ile ta��nabilir.

## Nas�l �al��t�r�l�r?
1. Proje dizininde `dotnet restore` ve `dotnet build` komutlar�n� �al��t�r�n.
2. Veritaban� i�in migration ve update komutlar�n� �al��t�r�n:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
3. Uygulamay� ba�lat�n:
   ```
   dotnet run --project App.Api
   ```
4. API endpointleri �zerinden CRUD i�lemlerini ger�ekle�tirin.

## Dosya ��eri�i
- `App.Api/Student.cs`: ��renci model s�n�f�.
- `App.Api/StudentRepository.cs`: Veritaban� i�lemleri i�in repository.
- `App.Api/AppDbContext.cs`: Entity Framework Core DbContext.
- `App.Api/Controllers/StudentsController.cs`: API controller.
- `App.Api/appsettings.json`: Veritaban� ba�lant� ayarlar�.
- `App.Api/Program.cs`: Uygulama ba�lang�� ve servis konfig�rasyonu.
