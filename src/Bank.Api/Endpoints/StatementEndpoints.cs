using Bank.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Endpoints;

public static class StatementEndpoints
{
    public static IEndpointRouteBuilder MapStatementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/accounts/{id:int}/statement",
                async (int id, [FromHeader(Name = "X-Customer-Id")] int customerId, string? search, StatementService service, CancellationToken ct) =>
                {
                    var statement = await service.GetStatementAsync(id, customerId, search, ct);
                    return statement is null ? Results.NotFound() : Results.Ok(statement);
                })
            .WithTags("Ekstre")
            .WithName("GetStatement")
            .WithSummary("Hesap ekstresini getirir");

        return app;
    }
}
