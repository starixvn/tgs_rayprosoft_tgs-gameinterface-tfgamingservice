namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record LogoutCommand(
	Uri ApiUrl,
	string OperatorId,
	string PrivateToken,
	string Member);
