namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record BetTransactionUnSettledResult
{
    /// <summary>
    /// 会员号
    /// </summary>
    public string MemberCode { get; init; } = default!;
    /// <summary>
    /// 注单号
    /// </summary>
    public string Id { get; init; } = default!;
    /// <summary>
    /// 下注时间
    /// </summary>
    public DateTimeOffset DateCreated { get; init; }
    /// <summary>
    /// 下注金额
    /// </summary>
    public decimal Amount { get; init; }
    public string AdditionalData { get; init; } = default!;
}
