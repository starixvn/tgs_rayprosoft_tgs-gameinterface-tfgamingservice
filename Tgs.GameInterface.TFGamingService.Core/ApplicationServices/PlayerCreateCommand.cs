namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string PrivateToken,
    string MemberCode);
