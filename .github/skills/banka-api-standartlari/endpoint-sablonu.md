# Endpoint şablonu

```csharp
public static class OrnekEndpoints
{
    public static IEndpointRouteBuilder MapOrnekEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/ornekler").WithTags("Örnekler");

        group.MapPost("/", async (OrnekRequest request, IOrnekService service, CancellationToken ct) =>
                (await service.OlusturAsync(request, ct))
                    .ToHttpResult(sonuc => Results.Created($"/ornekler/{sonuc.Id}", sonuc)))
            .WithName("CreateOrnek")
            .WithSummary("Yeni örnek oluşturur");

        return app;
    }
}
```

Servis imzası:

```csharp
Task<Result<OrnekResponse>> OlusturAsync(OrnekRequest request, CancellationToken ct);
```

Yeni `Map*Endpoints` metodu `Program.cs` içinde diğerlerinin yanına eklenir.
