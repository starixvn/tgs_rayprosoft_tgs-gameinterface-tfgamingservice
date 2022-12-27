namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string PrivateToken,
    string MemberCode);
