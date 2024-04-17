namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record GameHistorySettledViewModel(
	Uri ApiUrl,
	string PrivateToken,
	DateTime StartTime,
	DateTime EndTime,
	int Page,
	int PageSize);
