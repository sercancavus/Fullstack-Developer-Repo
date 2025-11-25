# E-Ticaret Uygulaması - Geliştirme Günlüğü

Bu depo; .NET 8 ile basit bir e-ticaret altyapısını adım adım kurarken tuttuğum ilerleme notlarını içeriyor. Amaç: temel domain modelini çıkarmak, veri katmanını hazırlamak, iki ayrı web projesi (kullanıcı ve admin) ile ileride genişletebileceğim bir temel oluşturmak.

---
## 1. Hedef
Kullanıcı, ürün, kategori, sipariş, yorum gibi çekirdek kavramları barındıran; ileride kimlik doğrulama, rol bazlı yetki, sipariş akışı, yorum moderasyonu ve yönetim paneli eklenebilecek bir başlangıç platformu oluşturmak.

---
## 2. Çözüm Yapısı
- **App.Data**: Entity tanımları + `AppDbContext` + seed veriler.
- **App.Mvc**: Son kullanıcı arayüzü (MVC Views + Controller’lar; şu an statik ürün listeleri var).
- **Admin.Mvc**: Yönetim / API (Swagger ile test edilebilir, henüz gerçek iş mantıkları yok).

---
## 3. Kronolojik İlerleme
Aşağıda sırayla yaptığım işler var:
1. `App.Data` class library oluşturuldu.
2. EF Core + SqlServer paketleri eklendi.
3. Çekirdek entity seti (Role, User, Category, Product, ProductImage, ProductComment, Order, OrderItem, CartItem) yazıldı.
4. İlişkiler ve navigation property’ler tanımlandı.
5. Gereksinimlerdeki alan kuralları (min/max, required vs.) eklendi.
6. `AppDbContext` oluşturulup DbSet’ler eklendi.
7. Seed: 3 rol + 1 admin kullanıcı + 10 kategori.
8. Rastgele zaman yerine tekrar üretilebilirlik için sabit seed tarihi kullanmaya geçtim.
9. Seed’de bulunan ama entity’de olmayan alanları (UserName, PasswordHash) temizledim.
10. App.Data hedefi net9 -> net8 (diğer projelerle hizalandı).
11. Her iki web projesine proje referansı eklendi.
12. Connection string + `AddDbContext` konfigürasyonları yapıldı.
13. App.Mvc’de ilk açılışta DB oluşturmak için `EnsureCreated()` çağrısı eklendi (ileride migration’a geçilecek).
14. Çift tanımlanan `CartItem` (ikincil tanım) kaldırıldı.
15. `User` entity’de validasyonlar sıkılaştırıldı (email max, password min vs.).
16. Kritik ilişkilerde `DeleteBehavior.Restrict` tercih edildi.
17. Ortak zaman alanlarına `GETUTCDATE()` varsayılanı verildi.
18. Ürün liste görselleri çıkarıldı, sade kutu tasarımına geçildi.
19. 10 adet örnek ürün (isim + kategori + fiyat + stok) Index ve Listing sayfalarına statik olarak kondu.
20. Navbar’a “Ürünler” bağlantısı eklendi.
21. Ürün kartlarına stok rozetleri eklendi.
22. Ürün detay rotası sadeleştirildi: `product/{categoryName}/{title}/{id}`.
23. View’de `RouteData` kullanımından kaynaklı hata yerine strongly-typed `ProductDetailViewModel` tanıtıldı.
24. Ürün detay sayfası ViewModel ile yeniden yazıldı (fiyat, stok, açıklama, sipariş butonu).
25. Footer minimal hale getirildi.
26. Layout esnek (flex) yapıya çekilerek footer’ın alta yapışması sağlandı.
27. Okunabilirlik için ufak düzenlemeler, açıklayıcı yorumlar eklendi.
28. README ilk kez toparlandı.
29. Gelecekte dinamikleşecek alanlara TODO notları eklendi.
30. Admin tarafında Swagger 500 sorunu çıktı; neden: klasik (View dönen) controller’lar ve bir iki fazladan parantez.
31. Admin controller’lar API mantığına çekildi (`ControllerBase`, `[ApiController]`, attribute routing).
32. Fazladan süslü parantez kaynaklı derleme hataları giderildi.
33. `HealthController` ile hızlı sağlık testi sağlandı.
34. Swagger koşullu olmaktan çıkarıldı, her ortamda açıldı.
35. Admin tarafına da `EnsureCreated()` eklendi (geçici çözüm).
36. WeatherForecast demo yapısı temizlendi.
37. Admin API için Category / Product / Comment / User / Home placeholder endpoint’leri eklendi.
38. App.Mvc: `ProductController` eklendi, `Create` (GET/POST), `Edit` (GET/POST), `Delete` (POST) aksiyonları yazıldı.
39. App.Mvc: `ProductCreateViewModel`, `ProductEditViewModel` eklendi (DataAnnotations ile validasyon); karşılık gelen `Create.cshtml` ve `Edit.cshtml` sayfaları oluşturuldu.
40. App.Mvc: `ProductCommentViewModel` ve `ProductController.Comment` (GET/POST) eklendi; `Views/Product/Comment.cshtml` ile form + validasyon sağlandı.
41. Admin.Mvc: `CategoryController` gerçek DbContext ile `Create`/`Edit`/`Delete` işlemlerini ViewModel üstünden yapacak şekilde güncellendi.

---
## 4. Entity Özeti (Kısa)
| Entity | Amaç |
|--------|------|
| Role | Roller (admin, seller, buyer) |
| User | Kullanıcı temel bilgileri + rol bağı |
| Category | Ürün kategorisi (renk, ikon, CreatedAt) |
| Product | Satıcı FK + kategori + fiyat + stok + durum |
| ProductImage | Ürüne ait görsel URL’leri |
| ProductComment | Yorum + yıldız + onay |
| CartItem | Kullanıcının sepetteki ürünleri |
| Order / OrderItem | Sipariş ana + satırlar |

> Sepet ve sipariş akışları henüz kodlanmadı.

---
## 5. DbContext Notları
- Bazı ilişkilerde silme kısıtı: `Restrict`.
- Zaman damgaları için SQL tarafında varsayılan atanıyor.
- Seed veriler şu an küçük ve sabit.

---
## 6. Arayüz (App.Mvc)
- Ürün listesi ve detay sayfası statik verilerle çalışıyor.
- Tasarım şu an prototip: Görsel yok, odak metinsel yapı.
- Stok ve fiyatlar hard-coded, ileride DB’den gelecek.

---
## 7. Form/Model Validasyonu
- ViewModel’lerde `Required`, `MinLength`, `MaxLength`, `Range` gibi attribute’lar kullanıldı.
- View’larda `asp-validation-summary` ve `asp-validation-for` ile hata gösterimi yapılıyor.
- Client-side validasyon için `_ValidationScriptsPartial` dahil edildi.
- Server-side validasyon için `ModelState.IsValid` kontrolleri ve uygun geri dönüşler mevcut.

---
## 8. Çözülen Sorunlar (Özet Tablo)
| Sorun | Kaynak | Çözüm |
|-------|--------|-------|
| `AppDbContext` yok | Referans eksik | ProjectReference eklendi |
| İkili `CartItem` | Çift tanım | Fazla tanım kaldırıldı |
| RouteData hatası | View’de doğrudan erişim | ViewModel yapısına geçildi |
| net9 uyumsuzluğu | Farklı TFM | net8’e çekildi |
| Seed alan uyuşmazlığı | Yanlış alan isimleri | Seed düzeltildi |
| Swagger 500 | UI controller + parantez hatası | API formatına dönüş + cleanup |

---
## 9. Planlanan Sonraki Adımlar
1. ASP.NET Identity (roller + kullanıcı kayıt / giriş).
2. `EnsureCreated()` yerine EF Migrations.
3. Repository + Service katmanları (iş mantığı ayrıştırma).
4. AutoMapper + DTO/ViewModel dönüşümleri.
5. Ürün arama / filtreleme / paging.
6. Sepet -> Sipariş tam akışı (stok düş, fiyat sabitle, OrderCode üret).
7. Yorum onay süreci (Admin API üzerinden).
8. Global exception handling + tutarlı JSON response modeli.
9. Serilog + yapılandırılmış loglar.
10. Test: xUnit + InMemory veya Testcontainers.
11. Admin API endpoint’lerine gerçek iş mantığı.


---
## 10. Çalıştırma
1. Connection string’leri kontrol et.
2. `App.Mvc` veya `Admin.Mvc`’yi başlangıç projesi seç.
3. İlk açılışta `EnsureCreated()` DB’yi kurar (geçici). Migration’a geçince kaldırılacak.
4. `https://localhost:<mvcPort>/` → ürün listesi.
5. `https://localhost:<adminPort>/swagger` → admin API test paneli.

---
## 11. Eksik / Bekleyenler
- Gerçek kimlik doğrulama & yetki yok.
- Sipariş & sepet iş mantığı yok.
- Yorum onay / moderasyon yok.
- Yönetim (kategori / ürün CRUD) henüz gerçek değil (Admin tarafında Product Delete vb. tamamlanacak).
- Testler ve CI/CD pipeline kurulmadı.
- Logging + hata yönetimi temel seviyede.

---
## 12. Genel Not
Temel iskelet hazır. Bundan sonraki kritik eşik: Identity + Migrations + gerçek servis katmanı ile dinamikleşme. README düzenli olarak güncellenecek.
