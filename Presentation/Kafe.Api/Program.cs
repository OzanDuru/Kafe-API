using Kafe.Application.Interfaces;
using Kafe.Application.Mapping;
using AutoMapper;
using Kafe.Application.Services.Abstract;
using Kafe.Application.Services.Concrete;
using Kafe.Persistence.Context;
using Kafe.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;   // AddOpenApi / MapOpenApi için
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
// ----------------------------------------------------------------------------
// Connection string'i appsettings.{Environment}.json içinden alıyoruz.
// MySQL için Pomelo provider kullanılıyor. MySqlServerVersion, EF Core'un
// sunucu sürümüne göre SQL üretmesini sağlar (özellikle migration/sql dialect).
// ----------------------------------------------------------------------------
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2)))
);

// Controllers
// ----------------------------------------------------------------------------
// Minimal API yerine klasik Controller tabanlı (attribute routing) API
// geliştirdiğimiz için Controller desteğini ekliyoruz.
// ----------------------------------------------------------------------------
builder.Services.AddControllers();

// Repository & Services
// ----------------------------------------------------------------------------
// Repository pattern: GenericRepository<T> ile temel CRUD erişimini soyutluyoruz.
// AddScoped: her HTTP request için bir instance üretir (DbContext ile uyumlu).
// ----------------------------------------------------------------------------
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<IMenuServices, MenuServices>();

// AutoMapper (v14 ile güvenli çağrı)
// ----------------------------------------------------------------------------
// Profil sınıfımız (GeneralMapping) AutoMapper.Profile'dan türemelidir.
// .Assembly overload'ı: profile'ların bulunduğu assembly'i tarayıp tüm
// profilleri otomatik bulur. Sürüm/overload farklarından doğan tip hatalarını
// önlemenin en sağlam yoludur.
// ----------------------------------------------------------------------------
builder.Services.AddAutoMapper(typeof(GeneralMapping).Assembly);

// Microsoft OpenAPI (OpenAPI json üretir)
// ----------------------------------------------------------------------------
// .NET 8/9 ile gelen resmi OpenAPI üreticisidir. /openapi/* endpointlerine
// JSON şema üretir. Scalar veya Swagger UI bu şemayı okuyup arayüz sağlar.
// ----------------------------------------------------------------------------
builder.Services.AddOpenApi();

// NOTE: Swagger kullanmıyorsan AddEndpointsApiExplorer gerekmez
// builder.Services.AddEndpointsApiExplorer(); // <-- kaldırıldı
// ----------------------------------------------------------------------------
// AddEndpointsApiExplorer() genelde Swashbuckle (Swagger) için gereklidir.
// Biz burada Microsoft.OpenApi + Scalar kullanıyoruz. Swagger eklemiyorsan
// bu satır gereksiz. Kullanacaksan Build'ten önce çağrılmalı.
// ----------------------------------------------------------------------------

var app = builder.Build();

// OpenAPI endpoint (örn. /openapi/v1.json)
// ----------------------------------------------------------------------------
// MapOpenApi() ile OpenAPI belgesini publish ediyoruz.
// Varsayılan rota: /openapi/v1.json (versioning'i ayrıca yapılandırabilirsin).
// Bu endpoint'i Scalar UI okuyacak.
// ----------------------------------------------------------------------------
app.MapOpenApi();

// Scalar UI
// ----------------------------------------------------------------------------
// Scalar.AspNetCore ile modern, hızlı bir OpenAPI UI yayınlıyoruz. Aşağıdaki
// ayarlar sadece UI görünümü ve davranışını etkiler. API logic'ine dokunmaz.
// ----------------------------------------------------------------------------
app.MapScalarApiReference(opt =>
{
    opt.Title = "Kafe API v1";
    opt.Theme = ScalarTheme.BluePlanet;

    // Eğer burada tip uyuşmazlığı hatası alırsan bu satırı sil.
    // (2.1.11’de bazı imzalar farklı olabiliyor)
    // opt.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);

    // Not: DefaultHttpClient'i set etmezsen Scalar kendi varsayılanlarını kullanır.
    // Genellikle ekstra ayara gerek yoktur.
});
app.MapGet("/", () => Results.Redirect("/scalar/v1"));

app.UseHttpsRedirection();
// ----------------------------------------------------------------------------
// HTTP -> HTTPS yönlendirmesi. Geliştirme ortamında dev sertifika ile, prod’da
// gerçek TLS sertifikası ile çalıştırmalısın.
// ----------------------------------------------------------------------------

app.UseAuthorization();
// ----------------------------------------------------------------------------
// Eğer Authentication/Authorization kullanıyorsan (JWT, Cookie vs.), ilgili
// middleware ve policy/attribute'larla birlikte burada devreye girer.
// Şu an sadece UseAuthorization var; ileride UseAuthentication eklenebilir.
// ----------------------------------------------------------------------------

app.MapControllers();
// ----------------------------------------------------------------------------
// Attribute routing kullanan tüm controller action’larını endpoint olarak
// haritalar. Minimal API endpoint’lerin varsa onlar için ayrı Map... çağrıları
// gerekir.
// ----------------------------------------------------------------------------

app.Run();
// ----------------------------------------------------------------------------
// Uygulamayı host eder ve Kestrel üzerinde dinlemeye başlar. Varsayılan
// portlar: http(s)://localhost:port (launchSettings.json ile değişebilir).
// ----------------------------------------------------------------------------

// Migration Komutları
// ----------------------------------------------------------------------------
// # İlk migrasyon
//  cd Infrastructure/Kafe.Persistence
// dotnet ef migrations add InitialCreate \
//   --startup-project ../../Presentation/Kafe.Api \
//   -o Migrations

// # Veritabanına uygula
//  dotnet ef database update \                                            
//   --project Infrastructure/Kafe.Persistence \
//   --startup-project Presentation/Kafe.Api
//----------------------------------------------------------------------------