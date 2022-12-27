namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string PrivateToken,
    string MemberCode);
