namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record BetTransactionSettledResult
{
    /// <summary>
    /// 会员号
    /// </summary>
    public string PlayerCode { get; init; } = default!;
    /// <summary>
    /// 注单号
    /// </summary>
    public string ExternalRoundId { get; init; } = default!;
    /// <summary>
    /// 下注时间
    /// </summary>
    public DateTimeOffset ExternalCreatedOn { get; init; }
    /// <summary>
    /// 结算时间
    /// </summary>
    public DateTimeOffset ExternalClosedOn { get; init; }
    /// <summary>
    /// 下注金额
    /// </summary>
    public decimal Bet { get; init; }
    /// <summary>
    /// 注单结果<br/>
    /// WIN = 赢<br/>
    /// LOSS = 输<br/>
    /// DRAW = 和<br/>
    /// CANCELLED = 取消
    /// </summary>
    public string? ResultStatus { get; init; }
    /// <summary>
    /// 输赢额<br/>
    /// WIN(赢) = 本金(bet amount) + 盈利(winnings)<br/>
    /// LOSS(输) = -本金(-bet amount)<br/>
    /// DRAW(和) = 本金(bet amount)<br/>
    /// PENDING(未结算) = null<br/>
    /// CANCELLED(取消) = null<br/>
    /// </summary>
    public decimal? Earnings { get; init; }

    public string AdditionalData { get; init; } = default!;
}
