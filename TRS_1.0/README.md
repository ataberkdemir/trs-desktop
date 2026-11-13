# Toplantı Randevu Sistemi (TRS 1.1)

C# Windows Forms ve .NET Framework 4.7.2 ile hazırlanmış masaüstü toplantı/randevu yönetimi projesi.

## Özellikler

- Yönetici ve kullanıcı oturumu
- Toplantı oluşturma, listeleme, düzenleme ve silme
- Katılımcı seçimi ve kullanıcı ana sayfası
- Yerel dosyaya temel işlem günlüğü

## Teknoloji ve açılış

- Windows Forms, C# ve .NET Framework 4.7.2
- ADO.NET / SQL Server
- Çözüm dosyası: `TRS 1.1.sln`
- Gerekli ortam: Windows ve .NET Framework 4.7.2 Developer Pack kurulu Visual Studio

`TRS 1.1/App.config` ve `TRS 1.1/Properties/Settings.settings` yerel `localhost` SQL Server örneği ve Windows Integrated Security kullanır. Kendi yerel SQL Server örneğin farklıysa `Data Source` değerini yerel ayarına göre değiştir. Bağlantı dizesinde SQL kullanıcı adı/parolası bulunmaz ve sertifika doğrulamasını atlayan `TrustServerCertificate=True` seçeneği yoktur. Uzak SQL sunucusuna bağlanacaksan güvenilir sertifika ve uygun kimlik doğrulama yapılandır.

## Güvenlik düzenlemeleri

Bu temizlenmiş kaynak kopyasında giriş kontrolü artık parolayı SQL sorgusunda karşılaştırmaz. Parolalar, ayrı rastgele salt kullanılarak PBKDF2-SHA256 ile türetilen değerler halinde saklanır; kullanıcı aramasındaki metin de SQL sorgusuna doğrudan eklenmez, parametre olarak gönderilir. Depo incelemesinde gerçek DB parolası, API anahtarı, token, özel anahtar, veritabanı dosyası veya varsayılan kullanıcı kaydı bulunmadı.

Bu düzenlemeler tam bir güvenlik sertifikası değildir. Masaüstü istemcisinin doğrudan SQL Server'a erişmesi üretim/internet kullanımı için uygun bir mimari sayılmaz. Gerçek toplantı veya kullanıcı verileriyle kullanmadan önce yetkilendirme, veritabanı izinleri, hata yönetimi ve ağ mimarisi ayrıca gözden geçirilmelidir.

Güncellenen `database/schema.sql`, yeni hesaplar için `admin_password_hash` ve `users_password_hash` alanlarını kullanır. Eski veritabanındaki düz metin parolaları yeni hash alanlarına kopyalama; mevcut hesapları güvenli bir parola yenileme süreciyle yeniden oluştur. Hash üretimi uygulamadaki `PasswordHasher.HashPassword(...)` metoduyla yerel ortamda yapılabilir. Gerçek parolayı veya üretilen kullanıcı kayıtlarını GitHub'a ekleme.

## Veritabanı şeması notu

`database/schema.sql` kaynak arşivinde bulunan kısmi şemadır; uygulamanın kullandığı tüm toplantı, profil ve kullanıcı alanlarını eksiksiz kuran doğrulanmış migration paketi değildir. Gerçek bir veritabanında çalıştırmadan önce şemayı incele ve yedek al.

## Görsel önizleme

`screenshots/01-giris-ekrani.png` boş giriş formunu gösterir. Kullanıcı/toplantı adı ve ayrıntıları içeren diğer görseller, verilerin kurgusal olduğu doğrulanamadığı için bu paylaşım paketine alınmadı.
