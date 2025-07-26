# TeamTaskTracker WebAPI

## Proje Açıklaması / Project Description
Bu proje, ekip görevlerini yönetmek için basit bir .NET 8 WebAPI uygulamasıdır. Görev ekleme, silme, güncelleme, filtreleme, sayfalama ve sıralama gibi temel özellikler içerir. Hatalar özel olarak loglanır.

This project is a simple .NET 8 WebAPI application for managing team tasks. It includes basic features such as adding, deleting, updating, filtering, paging, and sorting tasks. Errors are specially logged.

## Özellikler / Features
- Görev ekleme, silme, güncelleme / Add, delete, update tasks
- Tamamlanma durumuna göre filtreleme / Filter by completion status
- Sayfalama (pagination)
- Sıralama (Id, Title, DueDate, Priority) / Sorting
- Görevler için açıklama, tarih ve öncelik alanları / Description, date, and priority fields for tasks
- Hata yönetimi ve errorlogger.txt dosyasına loglama / Error management and logging to errorlogger.txt

## API Endpointleri / API Endpoints
- `GET /api/task` : Tüm görevleri getirir / Get all tasks
- `POST /api/task` : Yeni görev ekler / Add new task
- `PUT /api/task/{id}` : Görev günceller / Update task
- `DELETE /api/task/{id}` : Görev siler / Delete task
- `GET /api/task/filter?isCompleted=true|false` : Tamamlanma durumuna göre filtreler / Filter by completion status
- `GET /api/task/paged?page=1&pageSize=10&sortBy=priority&sortOrder=desc&includeDetails=true` : Sayfalama ve sıralama ile görevleri getirir / Get tasks with paging and sorting

## Kullanım / Usage
Proje .NET 8 ile çalışır. Swagger arayüzü ile API endpointlerini test edebilirsiniz.

The project runs with .NET 8. You can test API endpoints using the Swagger interface.

## Hata Yönetimi / Error Management
Beklenmeyen hatalar 500 Internal Server Error ile döner ve errorlogger.txt dosyasına kaydedilir.

Unexpected errors return 500 Internal Server Error and are logged to errorlogger.txt.

## Geliştirme Notları / Development Notes
- Şu anda bellek içi koleksiyon kullanılır. Gerçek projelerde veritabanı entegrasyonu önerilir.
- Kimlik doğrulama ve kullanıcı yönetimi eklenebilir.

Currently, in-memory collection is used. For real projects, database integration is recommended. Authentication and user management can be added.

---

## Eksik Özellikler ve Geliştirme Önerileri / Missing Features & Suggestions
- **Veritabanı Entegrasyonu:** Görevler kalıcı olarak saklanmalı, örneğin Entity Framework ile SQL Server veya SQLite kullanılabilir.  
**Database Integration:** Tasks should be stored persistently, e.g., using Entity Framework with SQL Server or SQLite.
- **Kimlik Doğrulama ve Kullanıcı Yönetimi:** JWT veya OAuth tabanlı kimlik doğrulama eklenmeli, görevler kullanıcıya özel olmalı.  
**Authentication & User Management:** Add JWT or OAuth-based authentication, tasks should be user-specific.
- **Unit Testler:** API endpointleri için birim testler ve entegrasyon testleri eklenmeli.  
**Unit Tests:** Add unit and integration tests for API endpoints.
- **API Rate Limiting:** Servisin aşırı kullanımı için rate limiting eklenebilir.  
**API Rate Limiting:** Add rate limiting for excessive usage.
- **Gelişmiş Hata Yönetimi ve Loglama:** Serilog veya NLog gibi kütüphanelerle merkezi loglama ve hata yönetimi yapılabilir.  
**Advanced Error Management & Logging:** Use Serilog or NLog for centralized logging and error management.
- **Docker Desteği:** Proje Docker ile containerize edilip kolayca dağıtılabilir.  
**Docker Support:** Containerize the project with Docker for easy deployment.
- **Swagger Açıklamaları:** Endpointler için detaylı Swagger açıklamaları ve örnekler eklenmeli.  
**Swagger Documentation:** Add detailed Swagger docs and examples for endpoints.
- **Frontend Entegrasyonu:** Bir frontend uygulaması ile entegre edilebilir (React, Angular, vb.).  
**Frontend Integration:** Can be integrated with a frontend app (React, Angular, etc.).

---
Geliştirici / Developer: [Sercan Çavuş]
