# Gorev12 - E-Ticaret Uygulaması (Açıklamalı ve Basit Anlatım)

Bu proje, .NET 8 ile hazırlanmış, temel e-ticaret işlevlerini ve yönetim panelini örnekleyen bir web uygulamasıdır. Kod yapısı ve işleyişi, yeni öğrenenlerin kolayca takip edebilmesi için sade ve açıklayıcı şekilde tasarlanmıştır.

---
## Proje Yapısı
- **App.Data**: Veritabanı nesneleri (entity class’ları) ve DbContext (veritabanı bağlantısı ve tablolar).
- **App.Mvc**: Kullanıcı arayüzü ve admin paneli (tüm controller, view ve işlemler burada).
- **Admin.Mvc**: (İsteğe bağlı) API veya ayrı yönetim paneli için temel yapı.

---
## Temel Özellikler
- **Kullanıcı Kayıt & Giriş:**
  - Herkes kayıt olabilir, kayıt olanlar "customer" rolüyle sisteme eklenir.
  - Sadece admin (admin@admin.com / admin123) yönetim paneline erişebilir.
- **Login olmadan:**
  - Ürünler ve anasayfa görüntülenemez, login zorunludur.
  - Hakkımızda ve İletişim sayfaları login olmadan da açıktır.
- **Admin Paneli:**
  - Ürün ve kategori ekleme, düzenleme, silme işlemleri yapılabilir.
  - Kullanıcılar ve kategoriler listelenir, düzenlenebilir.
- **Ürünler:**
  - Admin panelinden ürün eklenir, düzenlenir, silinir.
  - Ürün eklerken kategori seçimi zorunludur ve mevcut kategorilerden seçilir.
- **Kategoriler:**
  - Admin panelinden kategori eklenir, düzenlenir, silinir.
  - Kategori düzenle sayfasında o kategoriye ait ürünler listelenir.
- **Kullanıcılar:**
  - Admin panelinde tüm kullanıcılar tablo olarak listelenir.

---
## Kod Standartları ve Yapısı
- **Controller’lar:** Her işlev için ayrı controller (ör: ProductController, CategoryController, AuthController).
- **View’lar:** Her sayfa için ayrı view dosyası (ör: Create, Edit, Index, Delete).
- **Model Binding:** Formlardan gelen veriler doğrudan model nesnelerine bağlanır.
- **Validasyon:**
  - Hem sunucu tarafı (server-side) hem de istemci tarafı (client-side) validasyon aktif.
  - `[Required]`, `[Range]`, `[MaxLength]` gibi attribute’lar ile alan kontrolleri yapılır.
- **Temiz Kod:**
  - Gereksiz kod ve dosyalar kaldırıldı.
  - Her controller ve view’da sadece ilgili işlevler var.
  - Kodlar kısa, okunabilir ve açıklayıcı tutuldu.

---
## Kullanım ve Çalıştırma
1. **Gereksinimler:**
   - .NET 8 SDK
   - SQL Server Express veya LocalDB (Visual Studio ile otomatik gelir)
2. **Projeyi başlat:**
   - Visual Studio’da `App.Mvc` projesini başlat (F5 veya Ctrl+F5).
   - İlk açılışta veritabanı otomatik oluşur ve admin kullanıcısı eklenir.
3. **Giriş yap:**
   - Admin paneli için: `admin@admin.com` / `admin123`
   - Yeni kullanıcılar için: Kayıt ol sayfasından kayıt olabilirsin.
4. **Admin Paneli:**
   - Ürün/Kategori ekle, düzenle, sil işlemleri için menüleri kullan.
   - Kullanıcıları ve kategorileri listele, düzenle.
5. **Kullanıcı Tarafı:**
   - Giriş yaptıktan sonra ürünleri görebilir, detaylarını inceleyebilirsin.

---
## Öğrenmek İçin İpuçları
- **Controller ve View ilişkisini incele:** Her controller action’ı ilgili view dosyasını açar.
- **Model binding ve validasyon örneklerini incele:** Formlarda model binding ve validasyon nasıl çalışıyor bak.
- **Admin paneli ve kullanıcı tarafı ayrımını gözlemle:** Yetkilendirme ([Authorize]) ile roller nasıl ayrılmış bak.
- **Kodda açıklama ve örnekleri takip et:** Kodlar sade ve açıklayıcı tutuldu, her işlev için örnekler mevcut.

---
## Notlar
- Şifreler gerçek projede hash’lenmeli, burada öğrenme amaçlı düz metin tutuldu.
- Kodlar ve sayfalar yeni başlayanların kolayca anlayacağı şekilde sadeleştirildi.
- Herhangi bir hata veya eksik görürseniz, controller ve view dosyalarını inceleyerek kolayca düzeltebilirsiniz.

---
**Bu proje, .NET MVC ile temel CRUD, validasyon, kullanıcı yönetimi ve admin paneli mantığını öğrenmek isteyenler için sade ve açıklayıcı bir örnektir.**
