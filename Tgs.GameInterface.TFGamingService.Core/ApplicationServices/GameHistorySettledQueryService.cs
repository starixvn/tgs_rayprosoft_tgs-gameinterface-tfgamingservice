using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class GameHistorySettledQueryService : IQueryService<GameHistorySettledQuery, GameHistorySettledResult>
{
    private readonly IGameApiClient _client;

    public GameHistorySettledQueryService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistorySettledResult> QueryAsync(GameHistorySettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var bettingHistoryBets = await _client
                .BetTransactionSettledAsync(
                    apiUrl: query.ApiUrl,
                    privateToken: query.PrivateToken,
                    startTime: query.StartTime,
                    endTime: query.EndTime,
                    page: query.Page,
                    pageSize: query.PageSize,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!bettingHistoryBets.Any())
        {
            yield break;
        }

        foreach (var bettingHistoryBet in bettingHistoryBets)
        {
            var playerWinLoss = 0M;
            var settle = 0M;

            switch (bettingHistoryBet.ResultStatus)
            {
                case "WIN":
                    playerWinLoss = bettingHistoryBet.Earnings!.Value - bettingHistoryBet.Bet;
                    settle = bettingHistoryBet.Earnings!.Value;
                    break;

                case "LOSS":
                    playerWinLoss = bettingHistoryBet.Earnings!.Value;
                    break;

                case "DRAW":
                    break;

                case "CANCELLED":
                    settle = bettingHistoryBet.Bet;
                    break;

                default:
                    break;
            }

            yield return new GameHistorySettledResult(
                PlayerCode: bettingHistoryBet.PlayerCode,
                ExternalRoundId: bettingHistoryBet.ExternalRoundId,
                Status: bettingHistoryBet.ResultStatus!,
                ExternalCreatedOn: bettingHistoryBet.ExternalCreatedOn,
                ExternalClosedOn: bettingHistoryBet.ExternalClosedOn,
                Bet: bettingHistoryBet.Bet,
                ValidBet: bettingHistoryBet.Bet,
                PlayerWinLoss: playerWinLoss,
                Turnover: bettingHistoryBet.Bet,
                Settle: settle,
                AdditionalData: bettingHistoryBet.AdditionalData);
        }
    }
}
