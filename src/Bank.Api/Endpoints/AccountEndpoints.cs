using Bank.Api.Services;

namespace Bank.Api.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts").WithTags("Hesaplar");

        group.MapGet("/{id:int}", async (int id, IAccountService service, CancellationToken ct) =>
                (await service.GetAsync(id, ct)).ToHttpResult())
            .WithName("GetAccount")
            .WithSummary("Hesap bilgisini getirir");

        group.MapGet("/{id:int}/transfers", async (int id, ITransferService service, CancellationToken ct) =>
                (await service.GetHistoryAsync(id, ct)).ToHttpResult())
            .WithName("GetAccountTransfers")
            .WithSummary("Hesabın transfer geçmişini getirir");

        return app;
    }
}
