using Bank.Api.Contracts;
using Bank.Api.Services;

namespace Bank.Api.Endpoints;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/customers").WithTags("Müşteriler");

        group.MapPost("/{customerId:int}/internal-transfers",
                async (int customerId, InternalTransferRequest request, IInternalTransferService service, CancellationToken ct) =>
                    (await service.TransferAsync(customerId, request, ct))
                        .ToHttpResult(transfer => Results.Created($"/transfers/{transfer.Id}", transfer)))
            .WithName("CreateInternalTransfer")
            .WithSummary("Müşterinin kendi hesapları arasında virman yapar");

        return app;
    }
}
