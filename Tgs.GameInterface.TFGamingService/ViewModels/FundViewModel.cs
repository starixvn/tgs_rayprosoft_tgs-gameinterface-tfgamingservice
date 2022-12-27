namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string OperatorId,
    string PrivateToken,
    string Member,
    string RefTransactionCode,
    decimal Amount);
