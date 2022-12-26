using Tgs.GameInterface.TFGamingService.Core.Models;

namespace Tgs.GameInterface.TFGamingService.Core;

public interface IGameApiClient
{
    ValueTask<string?> MemberDirectLaunchAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default);

    ValueTask CreateMemberAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default);

    ValueTask<MemberBalanceResult> MemberBalanceAsync(Uri apiUrl, string privateToken, string memberCode, CancellationToken cancellationToken = default);

    ValueTask<DepositResult> DepositAsync(Uri apiUrl, string privateToken, string operatorId, string member, decimal amount, string referenceNo, CancellationToken cancellationToken = default);

    ValueTask<WithdrawResult> WithdrawAsync(Uri apiUrl, string privateToken, string operatorId, string member, decimal amount, string referenceNo, CancellationToken cancellationToken = default);

    ValueTask<bool> PartnerAccountLogoutAsync(Uri apiUrl, string privateToken, string member, string operatorId, CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string privateToken,
        string referenceNo,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<TransferStatusResult> TransferHistoryAsync(
        Uri apiUrl,
        string privateToken,
        string? referenceNo,
        int? page,
        int? pageSize,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetTransactionSettledResult> BetTransactionSettledAsync(
        Uri apiUrl,
        string privateToken,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetTransactionUnSettledResult> BetTransactionUnSettledAsync(
        Uri apiUrl,
        string privateToken,
        DateTime startTime,
        DateTime endTime,
        int page = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default);
}
