using Bank.Api.Contracts;
using Bank.Api.Services;

namespace Bank.Api.Endpoints;

public static class TransferEndpoints
{
    public static IEndpointRouteBuilder MapTransferEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/transfers").WithTags("Transferler");

        group.MapPost("/", async (TransferRequest request, ITransferService service, CancellationToken ct) =>
                (await service.TransferAsync(request, ct))
                    .ToHttpResult(transfer => Results.Created($"/transfers/{transfer.Id}", transfer)))
            .WithName("CreateTransfer")
            .WithSummary("Hesaplar arası para transferi yapar");

        return app;
    }
}
