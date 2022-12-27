namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record FundHistoryQuery(
    Uri ApiUrl,
    string PrivateToken,
    string? RefTransactionCode,
    int? Page,
    int? PageSize);
