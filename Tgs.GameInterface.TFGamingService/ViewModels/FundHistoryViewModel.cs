namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record FundHistoryViewModel(
	Uri ApiUrl,
	string PrivateToken,
	string? RefTransactionCode,
	int? Page,
	int? PageSize);
