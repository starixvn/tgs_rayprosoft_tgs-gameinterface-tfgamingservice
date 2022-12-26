namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record PlayerKickViewModel(
    Uri ApiUrl,
    string PrivateToken,
    string OperatorId,
    string Member);
