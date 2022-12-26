namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record FundStatusResult
{
    /// <summary>
    /// Tgs轉帳單號
    /// </summary>
    public string PartnerReferenceNo { get; init; } = default!;
    /// <summary>
    /// 例如:success
    /// </summary>
    public string Status { get; init; } = default!;
}
