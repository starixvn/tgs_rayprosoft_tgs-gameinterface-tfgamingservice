using Tgs.ServiceUtility;

namespace Tgs.GameInterface.TFGamingService.Core.ApplicationServices;

internal class GameUrlGetService(
    IGameApiClient client) : IGetService<GameUrlQuery, string?>
{
    public ValueTask<string?> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => client.MemberDirectLaunchAsync(query.ApiUrl, query.PrivateToken, query.MemberCode, cancellationToken);
}
