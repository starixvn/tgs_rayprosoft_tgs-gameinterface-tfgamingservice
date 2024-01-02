using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class FundInGetService(
    IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.WithdrawAsync(
            apiUrl: query.ApiUrl,
            privateToken: query.PrivateToken,
            operatorId: query.OperatorId,
            member: query.Member,
            amount: query.Amount,
            referenceNo: query.RefTransactionCode,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        var transferResults = await client
           .TransferHistoryAsync(query.ApiUrl, query.PrivateToken, query.RefTransactionCode, page: null, pageSize: null, cancellationToken: cancellationToken)
           .ToArrayAsync(cancellationToken)
           .ConfigureAwait(false);

        var transfer = transferResults.FirstOrDefault(x =>
            x.PartnerReferenceNo == query.RefTransactionCode
            && x.MemberCode == query.Member
            && x.Amount == query.Amount
            && x.TransactionType == "OUT");

        var transactionId = string.Empty;

        if (transfer != null)
            transactionId = transfer.Id.ToString();

        return new FundResult(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: transactionId,
            Balance: result.Balance);
    }
}
