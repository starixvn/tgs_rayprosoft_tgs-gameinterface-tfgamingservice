namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string PrivateToken,
    string MemberCode);
