# Kafe Yönetim Sistemi

.NET 7 ile geliştirilmiş, hem web arayüzü hem de API endpoints sunan kapsamlı bir kafe yönetim çözümü. Kafeler, siparişler, ürünler ve müşteri değerlendirmelerini yönetmek için tasarlanmıştır.

<img width="2240" height="1021" alt="Image" src="https://github.com/user-attachments/assets/2d7db273-2fdb-4a94-853f-ae87826b0230" />


<img width="2240" height="1029" alt="Image" src="https://github.com/user-attachments/assets/f4f41788-123a-44b1-adac-0e2ed6d5a033" />


<img width="2240" height="1026" alt="Image" src="https://github.com/user-attachments/assets/620dfa60-9a87-4013-a65f-394f3d61b216" />



## 🚀 Özellikler

- **Çoklu Kafe Yönetimi**
  - Birden fazla kafe oluşturma ve yönetme
  - Kafe özelliklerini ve ayarlarını özelleştirme
  - Bakım modu desteği

- **Ürün Yönetimi**
  - Ürün ekleme ve güncelleme
  - Kategori yönetimi
  - Fiyat yönetimi

- **Sipariş İşlemleri**
  - Sipariş oluşturma ve takip
  - Sipariş durumu yönetimi
  - Sipariş geçmişi

- **Müşteri Değerlendirmeleri**
  - Kafeler için değerlendirme sistemi
  - Puanlama sistemi
  - Müşteri geri bildirim yönetimi

- **Kullanıcı Yönetimi**
  - Kullanıcı kimlik doğrulama ve yetkilendirme
  - Rol tabanlı erişim kontrolü
  - Kullanıcı profilleri

## 🏗️ Mimari

Proje, aşağıdaki ana bileşenlerle temiz mimari (clean architecture) desenini takip eder:

- **CafeManagementSystem.WebApi**: REST API backend
- **CafeManagementSystem.Web**: MVC Web frontend
- **CafeManagementSystem.Business**: İş mantığı katmanı
- **CafeManagementSystem.Data**: Entity Framework Core ile veri erişim katmanı

## 🛠️ Teknoloji Yığını

- **Backend**: .NET 7
- **ORM**: Entity Framework Core
- **Kimlik Doğrulama**: JWT (JSON Web Token)
- **Frontend**: ASP.NET MVC, Bootstrap
- **Veritabanı**: SQL Server
- **API Dokümantasyonu**: Swagger/OpenAPI

## 📋 Gereksinimler

- .NET 7 SDK
- SQL Server
- Visual Studio 2022 veya VS Code

## 🚦 Başlangıç

1. **Projeyi klonlayın**
   ```bash
   git clone [repository-url]
   ```

2. **Veritabanı Bağlantısını Güncelleyin**
   - Web ve WebApi projelerindeki `appsettings.json` dosyalarını açın
   - Connection string'i kendi SQL Server'ınıza göre güncelleyin

3. **Veritabanı Migrationlarını Uygulayın**
   ```bash
   cd CafeManagementSystem.WebApi
   dotnet ef database update
   ```

4. **Projeleri Çalıştırın**
   - WebApi ve Web projelerini başlatın:
   ```bash
   dotnet run --project CafeManagementSystem.WebApi
   dotnet run --project CafeManagementSystem.Web
   ```

## 🔒 Güvenlik Özellikleri

- JWT tabanlı kimlik doğrulama
- Hassas bilgiler için veri koruma
- Rol tabanlı yetkilendirme
- Bakım modu ara yazılımı

## 🌟 Ana Bileşenler

### API Kontrolcüleri
- Auth Controller: Kullanıcı kimlik doğrulama ve yetkilendirme
- Cafes Controller: Kafe yönetim işlemleri
- Products Controller: Ürün yönetimi
- Orders Controller: Sipariş işlemleri
- Reviews Controller: Müşteri değerlendirme yönetimi

### İş Katmanı Servisleri
- Kullanıcı Yönetimi
- Kafe Yönetimi
- Sipariş İşlemleri
- Ürün Yönetimi
- Değerlendirme Sistemi
- Ayarlar Yönetimi

### Veri Katmanı
- Entity Framework Core
- Repository Pattern
- Unit of Work Pattern
- Veritabanı Migrations

## 📱 Kullanılabilir Endpoint'ler

### Web Arayüzü
- `/Account/Login`: Kullanıcı girişi
- `/Account/Register`: Yeni kullanıcı kaydı
- `/Cafe`: Kafe yönetimi
- `/Product`: Ürün yönetimi
- `/Order`: Sipariş yönetimi
- `/Customer`: Müşteri yönetimi

### API Endpoint'leri
- `/api/auth`: Kimlik doğrulama endpoint'leri
- `/api/cafes`: Kafe yönetimi
- `/api/products`: Ürün işlemleri
- `/api/orders`: Sipariş işlemleri
- `/api/reviews`: Değerlendirme yönetimi

## 🤝 Katkıda Bulunma

1. Repository'yi fork edin
2. Feature branch'inizi oluşturun
3. Değişikliklerinizi commit edin
4. Branch'inize push edin
5. Yeni bir Pull Request oluşturun

