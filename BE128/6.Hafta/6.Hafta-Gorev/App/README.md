# ASP.NET Core WebAPI Öðrenci Bilgi Sistemi

## Proje Amacý
Bu proje, .NET 8 ve ASP.NET Core WebAPI kullanýlarak temel bir Öðrenci Bilgi Sistemi oluþturmayý amaçlar. Öðrenci ekleme, listeleme, güncelleme ve silme iþlemleri API üzerinden yapýlabilir. Hem server-side hem de client-side validasyon örnekleri sunulmuþtur.

## Yapýlanlar
- **WebAPI Projesi:** .NET 8 tabanlý WebAPI projesi oluþturuldu.
- **Model:** `Student` sýnýfý ile öðrenci bilgileri ve doðrulama öznitelikleri tanýmlandý.
- **Veritabaný:** Entity Framework Core ile SQL Server üzerinde kalýcý veri saklama saðlandý.
- **Repository:** CRUD iþlemleri için `StudentRepository` katmaný oluþturuldu ve EF Core ile güncellendi.
- **Controller:** `StudentsController` ile API üzerinden CRUD iþlemleri saðlandý.
- **Validasyon:** Ad, soyad ve öðrenci numarasý için server-side validasyonlar eklendi.
- **Client Side:** HTML ve JavaScript ile form üzerinden client-side validasyon ve API baðlantýsý örnekleri sunuldu.

## Neler Yapýlabilir?
- **Kullanýcý Arayüzü:** Daha geliþmiþ bir frontend (React, Angular, Blazor) ile kullanýcý dostu arayüz geliþtirilebilir.
- **Kimlik Doðrulama:** JWT veya OAuth ile kullanýcý yetkilendirme ve kimlik doðrulama eklenebilir.
- **Unit Testler:** API ve repository katmanlarý için birim testler yazýlabilir.
- **Veritabaný Geliþtirme:** Öðrenciye ait dersler, notlar gibi ek tablolar ve iliþkiler eklenebilir.
- **API Dokümantasyonu:** Swagger/OpenAPI ile API dokümantasyonu geliþtirilebilir.
- **Deployment:** Proje bulut ortamýna (Azure, AWS) veya Docker ile taþýnabilir.

## Nasýl Çalýþtýrýlýr?
1. Proje dizininde `dotnet restore` ve `dotnet build` komutlarýný çalýþtýrýn.
2. Veritabaný için migration ve update komutlarýný çalýþtýrýn:
   ```
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
3. Uygulamayý baþlatýn:
   ```
   dotnet run --project App.Api
   ```
4. API endpointleri üzerinden CRUD iþlemlerini gerçekleþtirin.

## Dosya Ýçeriði
- `App.Api/Student.cs`: Öðrenci model sýnýfý.
- `App.Api/StudentRepository.cs`: Veritabaný iþlemleri için repository.
- `App.Api/AppDbContext.cs`: Entity Framework Core DbContext.
- `App.Api/Controllers/StudentsController.cs`: API controller.
- `App.Api/appsettings.json`: Veritabaný baðlantý ayarlarý.
- `App.Api/Program.cs`: Uygulama baþlangýç ve servis konfigürasyonu.
