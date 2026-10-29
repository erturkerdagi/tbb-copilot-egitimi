using Bank.Api.Contracts;
using Bank.Api.Services;

namespace Bank.Api.Endpoints;

public static class PaymentInstructionEndpoints
{
    public static IEndpointRouteBuilder MapPaymentInstructionEndpoints(this IEndpointRouteBuilder app)
    {
        var customers = app.MapGroup("/customers/{customerId:int}/payment-instructions").WithTags("Düzenli Ödeme");

        customers.MapPost("/",
                async (int customerId, CreatePaymentInstructionRequest request, IPaymentInstructionService service, CancellationToken ct) =>
                    (await service.CreateAsync(customerId, request, ct))
                        .ToHttpResult(i => Results.Created($"/customers/{customerId}/payment-instructions/{i.Id}", i)))
            .WithName("CreatePaymentInstruction")
            .WithSummary("Düzenli ödeme talimatı oluşturur");

        customers.MapGet("/",
                async (int customerId, IPaymentInstructionService service, CancellationToken ct) =>
                    Results.Ok(await service.ListAsync(customerId, ct)))
            .WithName("ListPaymentInstructions")
            .WithSummary("Müşterinin düzenli ödeme talimatlarını listeler");

        // Demo amaçlı: gerçek sistemde zamanlanmış bir iş tarafından çalıştırılır ve dışarıya açık olmaz.
        app.MapPost("/payment-instructions/run",
                async (DateOnly date, IPaymentInstructionRunner runner, CancellationToken ct) =>
                    Results.Ok(await runner.RunDueAsync(date, ct)))
            .WithTags("Düzenli Ödeme")
            .WithName("RunPaymentInstructions")
            .WithSummary("Verilen bankacılık gününde gerçekleşmesi gereken talimatları çalıştırır");

        return app;
    }
}
