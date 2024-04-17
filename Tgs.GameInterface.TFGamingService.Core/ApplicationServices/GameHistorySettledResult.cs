namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record GameHistorySettledResult(
	string PlayerCode,
	string ExternalRoundId,
	string Status,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal PlayerWinLoss,
	decimal Turnover,
	decimal Settle,
	string AdditionalData);
