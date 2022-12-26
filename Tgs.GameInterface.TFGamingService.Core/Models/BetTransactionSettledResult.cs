namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record BetTransactionSettledResult
{
    public string PlayerCode { get; init; } = default!;
    public string ExternalRoundId { get; init; } = default!;
    public DateTimeOffset ExternalCreatedOn { get; init; }

    public DateTimeOffset ExternalClosedOn { get; init; }
    public decimal Bet { get; init; }
    public decimal ValidBet { get; init; }
    public decimal PlayerWinLoss { get; init; }
    public decimal Turnover { get; init; }
    public decimal Settle { get; init; }
    public string AdditionalData { get; init; } = default!;
}
