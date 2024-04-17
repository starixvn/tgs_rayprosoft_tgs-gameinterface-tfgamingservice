using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class FundStatusGetService(
	IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
		=> client.FundStatusAsync(
				apiUrl: query.ApiUrl,
				privateToken: query.PrivateToken,
				referenceNo: query.RefTransactionCode,
				cancellationToken: cancellationToken);
}
