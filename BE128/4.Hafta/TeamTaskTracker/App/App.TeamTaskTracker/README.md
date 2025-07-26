# TeamTaskTracker WebAPI

## Proje A��klamas�
Bu proje, ekip g�revlerini y�netmek i�in basit bir .NET 8 WebAPI uygulamas�d�r. G�rev ekleme, silme, g�ncelleme, filtreleme, sayfalama ve s�ralama gibi temel �zellikler i�erir. Hatalar �zel olarak loglan�r.

## �zellikler
- G�rev ekleme, silme, g�ncelleme
- Tamamlanma durumuna g�re filtreleme
- Sayfalama (pagination)
- S�ralama (Id, Title, DueDate, Priority)
- G�revler i�in a��klama, tarih ve �ncelik alanlar�
- Hata y�netimi ve errorlogger.txt dosyas�na loglama

## API Endpointleri
- `GET /api/task` : T�m g�revleri getirir
- `POST /api/task` : Yeni g�rev ekler
- `PUT /api/task/{id}` : G�rev g�nceller
- `DELETE /api/task/{id}` : G�rev siler
- `GET /api/task/filter?isCompleted=true|false` : Tamamlanma durumuna g�re filtreler
- `GET /api/task/paged?page=1&pageSize=10&sortBy=priority&sortOrder=desc&includeDetails=true` : Sayfalama ve s�ralama ile g�revleri getirir

## Kullan�m
Proje .NET 8 ile �al���r. Swagger aray�z� ile API endpointlerini test edebilirsiniz.

## Hata Y�netimi
Beklenmeyen hatalar 500 Internal Server Error ile d�ner ve errorlogger.txt dosyas�na kaydedilir.

## Geli�tirme Notlar�
- �u anda bellek i�i koleksiyon kullan�l�r. Ger�ek projelerde veritaban� entegrasyonu �nerilir.
- Kimlik do�rulama ve kullan�c� y�netimi eklenebilir.

---

## Eksik �zellikler ve Geli�tirme �nerileri
- **Veritaban� Entegrasyonu:** G�revler kal�c� olarak saklanmal�, �rne�in Entity Framework ile SQL Server veya SQLite kullan�labilir.
- **Kimlik Do�rulama ve Kullan�c� Y�netimi:** JWT veya OAuth tabanl� kimlik do�rulama eklenmeli, g�revler kullan�c�ya �zel olmal�.
- **Unit Testler:** API endpointleri i�in birim testler ve entegrasyon testleri eklenmeli.
- **API Rate Limiting:** Servisin a��r� kullan�m� i�in rate limiting eklenebilir.
- **Geli�mi� Hata Y�netimi ve Loglama:** Serilog veya NLog gibi k�t�phanelerle merkezi loglama ve hata y�netimi yap�labilir.
- **Docker Deste�i:** Proje Docker ile containerize edilip kolayca da��t�labilir.
- **Swagger A��klamalar�:** Endpointler i�in detayl� Swagger a��klamalar� ve �rnekler eklenmeli.
- **Frontend Entegrasyonu:** Bir frontend uygulamas� ile entegre edilebilir (React, Angular, vb.).

---
Geli�tirici: [Sercan Çavuş]
