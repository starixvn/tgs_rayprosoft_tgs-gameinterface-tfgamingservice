namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record PlayerBalanceResult(
	bool IsPlayerExist,
	decimal? Balance);
