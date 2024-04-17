namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string PrivateToken,
	string MemberCode);
