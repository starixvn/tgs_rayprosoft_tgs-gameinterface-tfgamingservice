using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class PlayerBalanceGetService(
    IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.MemberBalanceAsync(
            apiUrl: query.ApiUrl,
            privateToken: query.PrivateToken,
            memberCode: query.MemberCode,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new PlayerBalanceResult(
             IsPlayerExist: result.IsPlayerExist,
             Balance: result.Balance);
    }
}
