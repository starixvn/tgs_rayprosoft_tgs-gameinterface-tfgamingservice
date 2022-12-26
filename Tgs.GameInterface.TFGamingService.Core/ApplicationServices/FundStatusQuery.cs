namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,    
    string PrivateToken,
    string RefTransactionCode);
