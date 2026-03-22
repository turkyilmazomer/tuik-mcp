# TÜİK MCP Server 🇹🇷

TÜİK (Türkiye İstatistik Kurumu) verilerine erişim sağlayan **Model Context Protocol (MCP)** sunucusu. .NET 8 ve Clean Architecture ile geliştirilmiştir.

Streamable HTTP transport ile çalışır. Tüm veriler gömülü JSON dosyalarından sunulur (TÜİK resmi yayınlarına dayalı).

> 🌐 **Public URL:** `https://tuik-mcp-production.up.railway.app/mcp`

## 📋 Veri Kategorileri & Araçlar

### 📊 Nüfus Verileri (2010-2024)
| Araç | Açıklama |
|------|----------|
| `get_population_by_year` | Belirtilen yılın nüfus verileri |
| `get_population_by_year_range` | Yıl aralığındaki nüfus trendi |
| `get_population_by_province` | İl bazında nüfus verileri |

### 📉 İşsizlik Verileri - Türkiye Geneli (2020-2025)
| Araç | Açıklama |
|------|----------|
| `get_unemployment_by_period` | Belirtilen dönemin işsizlik verileri |
| `get_unemployment_by_year` | Yıllık işsizlik trendi |
| `get_latest_unemployment` | En güncel işsizlik verileri |

### 📈 Enflasyon / TÜFE Verileri (2021-2025)
| Araç | Açıklama |
|------|----------|
| `get_inflation_by_period` | Belirtilen dönemin enflasyon verileri |
| `get_inflation_by_year` | Yıllık enflasyon trendi |
| `get_latest_inflation` | En güncel enflasyon verileri |

### 🎓 Eğitim Verileri - İl Bazında (2021-2023)
| Araç | Açıklama |
|------|----------|
| `get_education_by_province` | İl bazında eğitim göstergeleri |
| `get_education_by_year` | Belirtilen yılın tüm il eğitim verileri |
| `get_education_ranking` | Üniversite mezun oranına göre il sıralaması |
| `get_education_comparison` | İki il arasında eğitim karşılaştırması |

### 🏙️ İl Bazında İşsizlik (2021-2023)
| Araç | Açıklama |
|------|----------|
| `get_province_unemployment` | İl bazında işsizlik verileri |
| `get_province_unemployment_by_year` | Belirtilen yılın tüm il işsizlik verileri |
| `get_province_unemployment_ranking` | İşsizlik oranına göre il sıralaması |
| `compare_province_unemployment_education` | İl bazında işsizlik-eğitim çapraz analizi |

**Toplam: 17 MCP Tool**

## 🏗️ Mimari (Clean Architecture)

```
src/
├── TuikMcp.Domain/          # Entity'ler, Interface'ler
├── TuikMcp.Application/     # Uygulama servisleri, DTO'lar
├── TuikMcp.Infrastructure/  # Gömülü JSON veri, servis implementasyonları
└── TuikMcp.McpServer/       # MCP Server, Tool tanımları, HTTP transport
```

## 🚀 Kurulum & Çalıştırma

### Yerel Geliştirme

```bash
# Derleme
dotnet build

# Çalıştırma (http://localhost:5000/mcp)
dotnet run --project src/TuikMcp.McpServer
```

### Docker ile Çalıştırma

```bash
# Build
docker build -t tuik-mcp .

# Çalıştırma
docker run -p 8080:8080 tuik-mcp
```

Sunucu `http://localhost:8080/mcp` adresinde erişilebilir olacaktır.

## ☁️ Deployment

### Railway

1. [Railway](https://railway.app) hesabı oluşturun
2. GitHub reposunu bağlayın veya CLI kullanın:
```bash
npm i -g @railway/cli
railway login
railway init
railway up
```
Proje `railway.json` ile otomatik yapılandırılır.

✅ **Bu proje Railway'de yayında:** `https://tuik-mcp-production.up.railway.app/mcp`

### Fly.io

```bash
# Fly CLI kurulumu
curl -L https://fly.io/install.sh | sh

# Login & deploy
fly auth login
fly launch    # fly.toml otomatik algılanır
fly deploy
```
Public URL: `https://tuik-mcp.fly.dev/mcp`

### Google Cloud Run

```bash
gcloud run deploy tuik-mcp \
  --source . \
  --region europe-west1 \
  --allow-unauthenticated \
  --port 8080
```
Public URL: `https://tuik-mcp-<hash>-ew.a.run.app/mcp`

### Azure App Service

```bash
az webapp up --name tuik-mcp --runtime "DOTNET|8.0" --sku B1
```

## ⚙️ MCP Client Yapılandırması

### Claude Desktop / VS Code Copilot

```json
{
  "mcpServers": {
    "tuik": {
      "type": "http",
      "url": "https://tuik-mcp-production.up.railway.app/mcp"
    }
  }
}
```

### Continue (VS Code)

`~/.continue/config.yaml` dosyasına ekleyin:

```yaml
mcpServers:
  - name: tuik-mcp
    type: streamable-http
    url: https://tuik-mcp-production.up.railway.app/mcp
```

> **Yerel kullanım için:** URL yerine `http://localhost:5000/mcp` (dotnet run) veya `http://localhost:8080/mcp` (Docker) kullanın.

## 🔍 Health Check

Sunucu sağlık durumunu kontrol etmek için:

```bash
curl https://tuik-mcp-production.up.railway.app/health
# {"status":"healthy","service":"TuikMcp","version":"1.0.0"}
```

## 📊 Kullanım Örnekleri

MCP Client üzerinden şu sorular sorulabilir:

- "Türkiye'nin 2023 yılı nüfusu kaç?"
- "2018-2023 arası nüfus trendi nasıl?"
- "İstanbul'un nüfusu kaç?"
- "Güncel işsizlik oranı nedir?"
- "2024 yılı enflasyon verileri neler?"
- "Son açıklanan TÜFE verisi nedir?"
- "Ankara'nın eğitim istatistikleri neler?"
- "Üniversite mezun oranına göre en iyi 10 il hangisi?"
- "Şanlıurfa'da işsizlik ve eğitim arasındaki ilişki ne?"
- "İstanbul ve İzmir'in eğitim göstergelerini karşılaştır"
- "2023 yılında işsizlik oranı en yüksek iller hangileri?"

## 📡 Veri Kaynağı

Veriler TÜİK resmi yayınlarına dayalı **gömülü JSON veri setlerinden** sunulmaktadır. Harici API bağımlılığı yoktur, sunucu tamamen bağımsız çalışır.

> ⚠️ **Not:** Veriler TÜİK bültenlerinden derlenmiş olup genel eğilimleri ve büyüklük sıralarını doğru yansıtmaktadır. Ancak bazı rakamlar yuvarlanmış veya yaklaşık olabilir. Kesin veriler için [TÜİK resmi sitesi](https://data.tuik.gov.tr/) referans alınmalıdır.

**Kapsanan iller (eğitim & il işsizlik):** İstanbul, Ankara, İzmir, Bursa, Antalya, Adana, Konya, Gaziantep, Şanlıurfa, Diyarbakır, Mersin, Kayseri, Eskişehir, Trabzon, Samsun, Denizli, Malatya, Van, Erzurum, Mardin + Türkiye geneli

## 🛠️ Teknolojiler

- **.NET 8** (ASP.NET Core)
- **Model Context Protocol (MCP)** — `ModelContextProtocol` + `ModelContextProtocol.AspNetCore` 1.1.0
- **Streamable HTTP Transport**
- **Clean Architecture** (Domain → Application → Infrastructure → McpServer)
- **Docker** (multi-stage build)
- **Gömülü JSON veri** (Embedded Resources)

## 📝 Lisans

MIT