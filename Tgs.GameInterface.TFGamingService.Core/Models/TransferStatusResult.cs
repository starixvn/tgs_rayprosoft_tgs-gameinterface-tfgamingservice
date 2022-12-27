namespace Tgs.GameInterface.TFGamingService.Core.Models;

public record TransferStatusResult
{
    /// <summary>
    /// 編號 例如:2264090
    /// </summary>
    public int Id { get; init; }
    /// <summary>
    /// Tgs轉帳單號
    /// </summary>
    public string PartnerReferenceNo { get; init; } = default!;
    /// <summary>
    /// 申請的代理商名稱 例如:GPI_HKD
    /// </summary>
    public string OperatorName { get; init; } = default!;
    public string MemberCode { get; init; } = default!;
    /// <summary>
    /// IN:Tgs->Gp<br/>
    /// OUT:Gp->Tgs
    /// </summary>
    public string TransactionType { get; init; } = default!;
    public DateTimeOffset TransactionDate { get; init; }
    public decimal Amount { get; init; }
    /// <summary>
    /// 例如:success
    /// </summary>
    public string Status { get; init; } = default!;
    public decimal Balance { get; init; }
}
