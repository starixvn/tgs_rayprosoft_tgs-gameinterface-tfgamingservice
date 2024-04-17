using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class GameHistoryUnsettledQueryService(
	IGameApiClient client) : IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult>
{
	public async IAsyncEnumerable<GameHistoryUnsettledResult> QueryAsync(GameHistoryUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var openBets = await client
				.BetTransactionUnSettledAsync(
					apiUrl: query.ApiUrl,
					privateToken: query.PrivateToken,
					startTime: query.StartTime,
					endTime: query.EndTime,
					pageSize: query.PageSize,
					cancellationToken: cancellationToken)
				.ToArrayAsync(cancellationToken)
				.ConfigureAwait(false);

		if (openBets.Length == 0)
		{
			yield break;
		}

		foreach (var openBet in openBets)
		{
			yield return new GameHistoryUnsettledResult(
				PlayerCode: openBet.MemberCode,
				ExternalRoundId: openBet.Id,
				ExternalCreatedOn: openBet.DateCreated,
				Bet: openBet.Amount,
				ValidBet: openBet.Amount,
				AdditionalData: openBet.AdditionalData);
		}
	}
}
