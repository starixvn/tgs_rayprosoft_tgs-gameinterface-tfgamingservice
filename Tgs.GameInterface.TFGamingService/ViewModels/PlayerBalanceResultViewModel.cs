namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record PlayerBalanceResultViewModel(
	bool IsPlayerExist,
	decimal? Balance);
