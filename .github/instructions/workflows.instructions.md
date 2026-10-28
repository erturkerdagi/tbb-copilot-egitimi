---
applyTo: ".github/workflows/**"
---
# GitHub Actions kuralları

- Her workflow'da `permissions` açıkça tanımlanır; varsayılan `contents: read`.
- .NET sürümü `global.json` dosyasından alınır (`actions/setup-dotnet` + `global-json-file`).
- Sırlar yalnızca `${{ secrets.ADI }}` ile kullanılır; YAML'a düz yazılmaz, `echo` ile log'a basılmaz.
- Üretim ortamına dağıtım yapan job'lar `environment: production` ile onaya bağlanır.
- Üçüncü taraf action'lar sürüm etiketiyle (kurumsal ortamda commit SHA'sıyla) sabitlenir.
