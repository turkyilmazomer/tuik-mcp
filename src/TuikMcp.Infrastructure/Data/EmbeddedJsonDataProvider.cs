using System.Reflection;
using System.Text.Json;

namespace TuikMcp.Infrastructure.Data;

/// <summary>
/// Embedded JSON kaynaklarını okuyan yardımcı sınıf.
/// Tüm JSON verileri uygulama başlatıldığında bir kez yüklenir ve bellekte tutulur.
/// </summary>
public static class EmbeddedJsonDataProvider
{
    private static readonly Assembly Assembly = typeof(EmbeddedJsonDataProvider).Assembly;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Embedded JSON dosyasını okuyup belirtilen tipe deserialize eder.
    /// </summary>
    /// <typeparam name="T">Deserialize edilecek tip</typeparam>
    /// <param name="resourceFileName">Data klasöründeki dosya adı (örn: "population.json")</param>
    /// <returns>Deserialize edilmiş veri listesi</returns>
    public static List<T> Load<T>(string resourceFileName)
    {
        var resourceName = $"TuikMcp.Infrastructure.Data.{resourceFileName}";
        using var stream = Assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded resource '{resourceName}' bulunamadı. " +
                $"Mevcut kaynaklar: {string.Join(", ", Assembly.GetManifestResourceNames())}");

        using var reader = new StreamReader(stream);
        var json = reader.ReadToEnd();

        return JsonSerializer.Deserialize<List<T>>(json, JsonOptions)
            ?? throw new InvalidOperationException($"'{resourceName}' JSON dosyası boş veya geçersiz.");
    }
}
