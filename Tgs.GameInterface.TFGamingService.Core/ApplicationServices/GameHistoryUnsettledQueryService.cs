using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class GameHistoryUnsettledQueryService : IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult>
{
    private readonly IGameApiClient _client;

    public GameHistoryUnsettledQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryUnsettledResult> QueryAsync(GameHistoryUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var openBets = await _client
                .BetTransactionUnSettledAsync(
                    apiUrl: query.ApiUrl,
                    privateToken: query.PrivateToken,
                    startTime: query.StartTime,
                    endTime: query.EndTime,
                    page: query.Page,
                    pageSize: query.PageSize,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!openBets.Any())
        {
            yield break;
        }

        foreach (var openBet in openBets)
        {
            yield return new GameHistoryUnsettledResult(
                PlayerCode: openBet.PlayerCode,
                ExternalRoundId: openBet.ExternalRoundId,
                ExternalCreatedOn: openBet.ExternalCreatedOn,
                Bet: openBet.Bet,
                ValidBet: openBet.ValidBet,
                AdditionalData: JsonSerializer.Serialize(openBet));
        }
    }
}
