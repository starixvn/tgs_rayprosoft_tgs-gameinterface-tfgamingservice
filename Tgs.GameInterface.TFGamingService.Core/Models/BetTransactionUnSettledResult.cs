namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record BetTransactionUnSettledResult
{
    public string PlayerCode { get; init; } = default!;
    public string ExternalRoundId { get; init; } = default!;
    public DateTimeOffset ExternalCreatedOn { get; init; }
    public decimal Bet { get; init; }
    public decimal ValidBet { get; init; }
    public string AdditionalData { get; init; } = default!;
}
