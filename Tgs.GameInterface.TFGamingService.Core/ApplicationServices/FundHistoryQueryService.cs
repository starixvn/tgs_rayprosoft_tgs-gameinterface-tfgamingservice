using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class FundHistoryQueryService : IQueryService<FundHistoryQuery, FundHistoryResult>
{
    private readonly IGameApiClient _client;

    public FundHistoryQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<FundHistoryResult> QueryAsync(FundHistoryQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var fundHistorys = await _client
                .TransferHistoryAsync(
                    apiUrl: query.ApiUrl,
                    privateToken: query.PrivateToken,
                    referenceNo: query.RefTransactionCode,
                    page: query.Page,
                    pageSize: query.PageSize,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!fundHistorys.Any())
        {
            yield break;
        }

        foreach (var fundHistory in fundHistorys)
        {
            yield return new FundHistoryResult(
                RefTransactionCode: fundHistory.PartnerReferenceNo,
                TransactionId: fundHistory.Id.ToString(),
                Amount: fundHistory.Amount,
                OperatorName: fundHistory.OperatorName,
                MemberCode: fundHistory.MemberCode,
                TransactionType: fundHistory.TransactionType,
                TransactionDate: fundHistory.TransactionDate,
                Status: fundHistory.Status,
                Balance: fundHistory.Balance);
        }
    }
}
