using Tgs.GameInterface.TFGamingService.Protos;

namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record GameHistorySettledResultViewModel(
    string PlayerCode,
    string ExternalRoundId,
    GameHistoryStatus Status,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal ValidBet,
    decimal PlayerWinLoss,
    decimal Turnover,
    decimal Settle,
    string AdditionalData);
