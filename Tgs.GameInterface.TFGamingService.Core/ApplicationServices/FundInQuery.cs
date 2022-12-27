namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string OperatorId,
    string PrivateToken,
    string Member,
    string RefTransactionCode,
    decimal Amount);
