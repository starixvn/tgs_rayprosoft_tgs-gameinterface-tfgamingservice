namespace Tgs.GameInterface.TFGamingService.ViewModels;

public record FundHistoryResultViewModel
{
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 转账编号
    /// </summary>
    public string RefTransactionCode { get; init; } = default!;
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 编号
    /// </summary>
    public string TransactionId { get; init; } = default!;
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 金額
    /// </summary>
    public decimal Amount { get; init; }
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 平台
    /// </summary>
    public string OperatorName { get; init; } = default!;
    /// <summary>
    /// 会员ID
    /// </summary>
    public string MemberCode { get; init; } = default!;
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 	注单类型<br/>
    /// IN:存款 / OUT:提款
    /// </summary>
    /// <value>
    /// </value>
    public string TransactionType { get; init; } = default!;
    /// <summary>
    /// 日期
    /// </summary>
    public DateTimeOffset TransactionDate { get; init; }
    /// <summary>
    /// 雷火後台 > 会员转账记录 > 	状况
    /// </summary>
    public string Status { get; init; } = default!;
    /// <summary>
    /// 轉帳後金額
    /// </summary>
    /// <value>
    /// The balance.
    /// </value>
    public decimal Balance { get; init; }
}
