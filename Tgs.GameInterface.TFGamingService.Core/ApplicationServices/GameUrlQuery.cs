namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record GameUrlQuery(
	Uri ApiUrl,
	string PrivateToken,
	string MemberCode);
