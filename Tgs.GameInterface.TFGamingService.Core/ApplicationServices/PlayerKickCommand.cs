namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record PlayerKickCommand(
    Uri ApiUrl,
    string PrivateToken,
    string OperatorId,
    string Member);
