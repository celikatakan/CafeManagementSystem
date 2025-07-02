# Cafe Yorum Sistemi

Bu proje, kullanıcıların cafelere yorum yapabilmesi için AJAX tabanlı bir yorum sistemi içerir.

## Özellikler

- ✅ Kullanıcılar cafelere yorum yapabilir
- ✅ 1-5 arası puanlama sistemi
- ✅ Maksimum 500 karakter yorum
- ✅ Her kullanıcı bir cafe için sadece bir yorum yapabilir
- ✅ Yorumlar tarih sırasına göre listelenir
- ✅ Modern ve responsive tasarım
- ✅ JWT token tabanlı kimlik doğrulama

## API Endpoints

### Yorum Ekleme
```
POST /api/Reviews
Content-Type: application/json
Authorization: Bearer {JWT_TOKEN}

{
  "cafeId": 1,
  "rating": 5,
  "comment": "Harika bir cafe! Çok beğendim."
}
```

### Yorumları Getirme
```
GET /api/Reviews/cafe/{cafeId}
Authorization: Bearer {JWT_TOKEN}
```

## Kullanım

### 1. Giriş Yapma
Önce sisteme giriş yapmanız gerekiyor. JWT token'ınızı alın.

### 2. Cafe Detay Sayfasına Gitme
Cafe detay sayfasında yorum bölümü otomatik olarak yüklenecektir.

### 3. Yorum Yapma
- Puanınızı seçin (1-5)
- Yorumunuzu yazın (maksimum 500 karakter)
- "Yorum Gönder" butonuna tıklayın

### 4. Yorumları Görüntüleme
Yorumlar otomatik olarak yüklenir ve en yeni yorumlar üstte görünür.

## Teknik Detaylar

### Backend
- **ReviewManager**: Yorum işlemlerini yönetir
- **ReviewsController**: API endpoint'lerini sağlar
- **ReviewEntity**: Veritabanı modeli
- **CreateReviewDto**: Yorum ekleme için veri transfer objesi
- **ReviewDto**: Yorum listeleme için veri transfer objesi

### Frontend
- **cafeReviews.js**: AJAX işlemlerini yönetir
- **reviews.css**: Modern ve responsive tasarım
- **Details.cshtml**: Yorum formu ve listesi

### Güvenlik
- JWT token tabanlı kimlik doğrulama
- Her kullanıcı bir cafe için sadece bir yorum yapabilir
- Input validation ve sanitization

## Kurulum

1. Projeyi build edin:
```bash
dotnet build
```

2. Veritabanını güncelleyin:
```bash
dotnet ef database update
```

3. Uygulamayı çalıştırın:
```bash
dotnet run
```

## Dosya Yapısı

```
CafeManagementSystem/
├── CafeManagementSystem.Business/
│   └── Operations/Review/
│       ├── ReviewManager.cs
│       ├── IReviewService.cs
│       └── Dtos/
│           ├── CreateReviewDto.cs
│           └── ReviewDto.cs
├── CafeManagementSystem.Data/
│   ├── Entities/ReviewEntity.cs
│   └── Repositories/
│       ├── IRepository.cs
│       └── Repository.cs
└── CafeManagementSystem.WebApi/
    ├── Controllers/ReviewsController.cs
    ├── Views/Cafe/Details.cshtml
    ├── wwwroot/js/cafeReviews.js
    └── wwwroot/css/reviews.css
```

## Hata Kodları

- **401**: Kimlik doğrulama gerekli
- **400**: Geçersiz veri veya zaten yorum yapılmış
- **404**: Cafe bulunamadı

## Özelleştirme

### CSS Stillerini Değiştirme
`wwwroot/css/reviews.css` dosyasını düzenleyerek tasarımı özelleştirebilirsiniz.

### JavaScript İşlevselliğini Değiştirme
`wwwroot/js/cafeReviews.js` dosyasını düzenleyerek davranışı özelleştirebilirsiniz.

### API Endpoint'lerini Değiştirme
`Controllers/ReviewsController.cs` dosyasını düzenleyerek API'yi özelleştirebilirsiniz.

## Destek

Herhangi bir sorun yaşarsanız, lütfen issue açın veya geliştirici ile iletişime geçin. 