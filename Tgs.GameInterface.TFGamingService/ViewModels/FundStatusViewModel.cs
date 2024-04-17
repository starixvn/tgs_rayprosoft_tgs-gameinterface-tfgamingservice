namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record FundStatusViewModel(
	Uri ApiUrl,
	string PrivateToken,
	string RefTransactionCode);
