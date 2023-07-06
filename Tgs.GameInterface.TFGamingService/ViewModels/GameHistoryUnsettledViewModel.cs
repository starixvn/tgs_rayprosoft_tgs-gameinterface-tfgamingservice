namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record GameHistoryUnsettledViewModel(
    Uri ApiUrl,
    string PrivateToken,
    DateTime StartTime,
    DateTime EndTime,
    int Page,
    int PageSize);
